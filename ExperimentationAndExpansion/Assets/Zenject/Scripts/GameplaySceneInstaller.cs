
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject.Scripts.Coins;
using Zenject.Scripts.UI;

namespace Zenject.Scripts
{
    public class GameplaySceneInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private BagManager _bagManagerPrefab;
        [SerializeField] private BagView _bagView;

        [Title("Coins Data")] 
        [SerializeField] private CoinConfig _coinConfig; 

        #endregion
        
        public override void InstallBindings()
        {
            CreateAllCoins();
            CreateAndInjectBagManager();
            InjectBagView();
        }

        private void InjectBagView()
        {
            Container.Bind<BagView>().FromInstance(_bagView).AsSingle();
        }

        private void CreateAndInjectBagManager()
        {
            BagManager bagManager = Container.InstantiatePrefabForComponent<BagManager>(_bagManagerPrefab);
            Container.Bind<BagManager>().FromInstance(bagManager);
        }

        #region InjectCoins

        private void CreateAllCoins()
        {
            List<BaseCoin> _allCoins = new List<BaseCoin>();
            foreach (var coinType in Assembly.GetAssembly(typeof(BaseCoin)).GetTypes().Where(myType => myType.IsSubclassOf(typeof(BaseCoin))))
            {
                var coin = Activator.CreateInstance(coinType) as BaseCoin;
                if (coin == null)
                {
                    return;
                }
                
                coin.Init(_coinConfig.Coins.First(coinData => coinData.CoinType == coin.CoinsType));
                _allCoins.Add(coin);
            }
            
            Container.Bind<List<BaseCoin>>().FromInstance(_allCoins);
            // CreateAndInjectGoldenCoin();
            // CreateAndInjectDeathCoin();
            // CreateAndInjectWoodCoin();
        }

        // private void CreateAndInjectWoodCoin()
        // {
        //     CoinData coinData = _coinConfig.Coins.First(coin => coin.CoinType == CoinsEnum.WoodCoin);
        //     WoodenCoin woodCoin = new WoodenCoin(coinData);
        //     TInjectCoin(woodCoin);
        // }
        //
        // private void CreateAndInjectDeathCoin()
        // {
        //     CoinData coinData = _coinConfig.Coins.First(coin => coin.CoinType == CoinsEnum.BlackCoin);
        //     BlackCoin blackCoin = new BlackCoin(coinData);
        //     TInjectCoin(blackCoin);
        // }
        //
        // private void CreateAndInjectGoldenCoin()
        // {
        //     CoinData coinData = _coinConfig.Coins.First(coin => coin.CoinType == CoinsEnum.GoldCoin);
        //     GoldCoin goldCoin = new GoldCoin(coinData);
        //     TInjectCoin(goldCoin);
        // }
        //
        // private void TInjectCoin<T>(T typeCoin) where T : BaseCoin
        // {
        //     Container.Bind<T>().FromInstance(typeCoin);
        //     Container.Bind<BaseCoin>().To<T>().AsSingle();
        // }

        #endregion
    }
}
