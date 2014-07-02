﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Embedded;

namespace FelicaData
{
    abstract public class RavenData : IDisposable
    {
        protected IDocumentStore DocumentStore { get; set; }

        /// <summary>
        /// 変更フラグ
        /// </summary>
        private Dictionary<Type, bool> changedFlags = new Dictionary<Type, bool>();

        protected RavenData(string connectionStringName)
        {
            this.DocumentStore = new EmbeddableDocumentStore
            {
                ConnectionStringName = connectionStringName
            };

            this.DocumentStore.Initialize(); // 初期化は時間が掛かる
        }

        protected T Create<T>(T data)
            where T : RavenModel
        {
            data.Id = 0; // アップデートにならないように Id を無効化

            this.Store(data);

            return data;
        }

        protected void Update<T>(T data)
            where T : RavenModel
        {
            this.Store(data);
        }

        private void Store<T>(T data)
            where T : class
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                session.Store(data);
                session.SaveChanges();
            }

            this.OnChanged(typeof(T));
        }

        protected List<T> Query<T>()
            where T : class
        {
            return this.Query<T>(x => true);
        }

        protected List<T> Query<T>(Func<T, bool> predicate)
            where T : class
        {
            // 変更があった場合、書き込みを待機
            if (this.changedFlags.ContainsKey(typeof(T)) &&
                this.changedFlags[typeof(T)])
            {
                return this.QueryBlocking(predicate);
            }

            else
            {
                return this.QueryNonBlocking(predicate);
            }
        }

        /// <summary>
        /// 書き込みを待機してクエリを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private List<T> QueryBlocking<T>(Func<T, bool> predicate)
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                var result = session.Query<T>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite()) // 書き込みを待機
                    .Where(predicate)
                    .ToList();

                // 変更を書き込み済み
                this.changedFlags[typeof(T)] = false;

                return result;
            }
        }

        /// <summary>
        /// 書き込みを待機せずクエリを実行する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private List<T> QueryNonBlocking<T>(Func<T, bool> predicate)
        {
            using (var session = this.DocumentStore.OpenSession())
            {
                return session.Query<T>()
                    .Where(predicate)
                    .ToList();
            }
        }

        protected void Delete<T>(T data)
            where T: class
        {
            if (data == null) { return; }

            using (var session = this.DocumentStore.OpenSession())
            {
                session.Delete<T>(data);
            }
        }


        #region Changed Event

        public EventHandler<Type> Changed;

        protected void OnChanged(Type type)
        {
            this.changedFlags[type] = true;

            if (this.Changed != null)
            {
                this.Changed(this, type);
            }
        }

        #endregion

        #region Dispose Finalize パターン
 
        /// <summary>
        /// 既にDisposeメソッドが呼び出されているかどうかを表します。
        /// </summary>
        private bool disposed = false;
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているすべてのリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 クラスのインスタンスがGCに回収される時に呼び出されます。
        /// </summary>
        ~RavenData()
        {
            this.Dispose(false);
        }
 
        /// <summary>
        /// ConsoleApplication1.DisposableClass1 によって使用されているアンマネージ リソースを解放し、オプションでマネージ リソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージ リソースとアンマネージ リソースの両方を解放する場合は true。アンマネージ リソースだけを解放する場合は false。 </param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }
            this.disposed = true;
 
            if (disposing)
            {
                // マネージ リソースの解放処理をこの位置に記述します。
                this.DocumentStore.Dispose();
            }
            // アンマネージ リソースの解放処理をこの位置に記述します。
        }
 
        /// <summary>
        /// 既にDisposeメソッドが呼び出されている場合、例外をスローします。
        /// </summary>
        /// <exception cref="System.ObjectDisposedException">既にDisposeメソッドが呼び出されています。</exception>
        protected void ThrowExceptionIfDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
 
        /// <summary>
        /// Dispose Finalize パターンに必要な初期化処理を行います。
        /// </summary>
        private void InitializeDisposeFinalizePattern()
        {
            this.disposed = false;
        }
 
        #endregion
    }
}