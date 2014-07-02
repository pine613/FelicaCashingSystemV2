﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            this.BuyCommand = new DelegateCommand<int>(this.Buy);
        }

        public bool IsAdmin
        {
            get
            {
                return App.Current.User != null &&
                    App.Current.User.IsAdmin;
            }
        }

        public ICommand BuyCommand { get; set; }
        private void Buy(int money)
        {
            Debug.WriteLine("Buy money = " + money.ToString());

            if (money <= 0)
            {
                throw new ArgumentOutOfRangeException("money");
            }



            // 購入処理
            if (App.Current.User != null)
            {
                if (App.Current.UserData.Buy(App.Current.User.Id, money))
                {
                    Debug.WriteLine("Buying succeed");

                    this.ShowMessageBox(
                        "「 " + money + " 」 円の商品を購入しました。\n" +
                        "残高は 「 " + (App.Current.User.Money - money) + " 」 円です。",
                        "購入成功");
                }
                else
                {
                    Debug.WriteLine("Buying error");
                    this.ShowMessageBox("商品の購入でエラーが発生しました。", "購入失敗");
                }
            }
        }
    }
}
