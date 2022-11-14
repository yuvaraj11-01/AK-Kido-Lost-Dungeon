using RestSharp;
using Org.MetaFab.Api;
using Org.MetaFab.Client;
using Org.MetaFab.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MetafabManager 
{
    #region Keys & Const

    private const string game_PK = "game_pk_cTN39T8WlVQd+mwbnA3RvqNqa2494w7G3wdXWPkbQQd0Dwl1aWQjWZ6ufLZLT0Lk";
    private const string game_SK = "game_sk_+vlwNqqnv0duEmY6iam96Y+hDasgsqjEjrOlZ9Zgk+Ma1FyoTqvzCOIiTuUi9pdc";
    private const string game_ps = "NBBCKtzY";
    private const string BaseUri = "https://api.trymetafab.com";

    #endregion

    #region Player Manager

    public static void LoginPlayer(string userName,string password, Action<bool> Callback=null)
    {
        // Authenticate
        //  - player existed
        //      - success result
        //  - Player not existed
        //      - create new player
        //          - success result

        Configuration.Default.BasePath = BaseUri;

        Configuration.Default.Username = userName;
        Configuration.Default.Password = password;

        AuthenticatePlayer(Callback, () =>
        {
            CreateNewPlayer(Callback, () =>
            {
                Callback?.Invoke(false);
            });
        });

    }

    static void AuthenticatePlayer(Action<bool> SuccessCallback = null, Action FailureCallback = null)
    {
        var apiInstance = new PlayersApi(Configuration.Default);

        try
        {
            AuthPlayer200Response result = apiInstance.AuthPlayer(game_PK);
            Debug.Log(result);
            Configuration.Default.AccessToken = result.AccessToken;
            StaticPlayerData.playerID = result.Id;
            StaticPlayerData.walletID = result.Wallet.Id;
            StaticPlayerData.walletAddress = result.Wallet.Address;
            SuccessCallback?.Invoke(true);
        }
        catch(ApiException e)
        {
            Debug.Log("Exception when calling PlayersApi.AuthPlayer: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            FailureCallback?.Invoke();
        }

    }

    static void CreateNewPlayer(Action<bool> SuccessCallback = null, Action FailureCallback = null)
    {
        var apiInstance = new PlayersApi(Configuration.Default);

        var createPlayerRequest = new CreatePlayerRequest(Configuration.Default.Username,
            Configuration.Default.Password);

        try
        {
            // Create player
            AuthPlayer200Response result = apiInstance.CreatePlayer(game_PK, createPlayerRequest);
            Debug.Log(result);
            Configuration.Default.AccessToken = result.AccessToken;
            StaticPlayerData.playerID = result.Id;
            StaticPlayerData.walletID = result.Wallet.Id;
            StaticPlayerData.walletAddress = result.Wallet.Address;
            SetPlayerData(new ProtectedPlayerData() { WeaponEquipedID = "", Score = "0" });
            GetCurrencyBalance();
            MintCurrency(5);
            SuccessCallback?.Invoke(true);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling PlayersApi.CreatePlayer: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            FailureCallback?.Invoke();
        }

    }

    public static void SetPlayerData(ProtectedPlayerData privateData = null)
    {
        var apiInstance = new PlayersApi(Configuration.Default);
        var playerId = StaticPlayerData.playerID;  // string | Any player id within the MetaFab ecosystem.
        var xAuthorization = game_SK;  // string | The `secretKey` of a specific game or the `accessToken` of a specific player.
        var setPlayerDataRequest = new SetPlayerDataRequest(privateData); // SetPlayerDataRequest | 

        try
        {
            // Set player data
            GetPlayerData200Response result = apiInstance.SetPlayerData(playerId, xAuthorization, setPlayerDataRequest);
            Debug.Log(result);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling PlayersApi.SetPlayerData: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }

    public static void GetPlayerData()
    {
        var apiInstance = new PlayersApi(Configuration.Default);
        var playerId = StaticPlayerData.playerID; 

        try
        {
            // Get player data
            GetPlayerData200Response result = apiInstance.GetPlayerData(playerId);
            Debug.Log(result);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling PlayersApi.GetPlayerData: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }


    #endregion

    #region Exchange Offer
    public static void GetExchangeOffers(Action<List<ExchangeOffer>> Offers)
    {
        // Get Exchanges
        //  - Get offers

        GetExchanges((res) =>
        {
            GetOffers(res.Id,Offers);
        });

    }

    static void GetExchanges(Action<GetExchanges200ResponseInner> Callback)
    {
        var apiInstance = new ExchangesApi(Configuration.Default);
        try
        {
            // Get exchanges
            List<GetExchanges200ResponseInner> result = apiInstance.GetExchanges(game_PK);
            Debug.Log(result);
            Callback?.Invoke(result[0]);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ExchangesApi.GetExchanges: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }

    static void GetOffers(string exchangeID,Action<List<ExchangeOffer>> callback)
    {
        var apiInstance = new ExchangesApi(Configuration.Default);

        var exchangeId = exchangeID;  // string | Any exchange id within the MetaFab ecosystem.

        try
        {
            // Get exchange offers
            List<ExchangeOffer> result = apiInstance.GetExchangeOffers(exchangeId);
            callback?.Invoke(result);
            foreach (var item in result)
            {
                Debug.Log(item);
            }
            
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ExchangesApi.GetExchangeOffers: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }

    public static void BuyOffer(string offerID,Action<bool> Callaback)
    {
        GetExchanges((res) =>
        {
            UseExchangeOffer(res.Id,offerID, Callaback);
        });
        
    }

    static void UseExchangeOffer(string exchangeID,string offerID, Action<bool> Callaback)
    {
        var apiInstance = new ExchangesApi(Configuration.Default);

        var exchangeId = exchangeID;  // string | Any exchange id within the MetaFab ecosystem.
        var exchangeOfferId = offerID;  // string | Any offer id for the exchange. Zero, or a positive integer.
        var xAuthorization =  Configuration.Default.AccessToken; // string | The `secretKey` of a specific game or the `accessToken` of a specific player.
        var xPassword = Configuration.Default.Password;  // string | The password of the authenticating game or player. Required to decrypt and perform blockchain transactions with the game or player primary wallet.

        try
        {
            // Use exchange offer
            TransactionModel result = apiInstance.UseExchangeOffer(exchangeId, exchangeOfferId, xAuthorization, xPassword);
            Debug.Log(result);
            Callaback?.Invoke(true);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ExchangesApi.UseExchangeOffer: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            Callaback?.Invoke(false);
        }
    }

    #endregion

    #region Collection Manager

    public static List<CollectionItem> GetItemBalance()
    {
        var result = new List<CollectionItem>();

        GetCollections((res) =>
        {
            GetCollectionItemBalances(res.Id, (res2) =>
            {
                foreach (var item in res2)
                {
                    result.Add(new CollectionItem() { ID = int.Parse(item.Key), count =(int)item.Value });
                }
            });
        });
        return result;
    }

    static void GetCollectionItemBalances(string collectionID, Action<Dictionary<string, decimal>> callback)
    {
        var apiInstance = new ItemsApi(Configuration.Default);
        var collectionId = collectionID;  // string | Any collection id within the MetaFab ecosystem.
        var walletId = StaticPlayerData.walletID;  // string | Any wallet id within the MetaFab ecosystem. (optional) 

        try
        {
            // Get collection item balances
            Dictionary<string, decimal> result = apiInstance.GetCollectionItemBalances(collectionId, walletId: walletId);
            Debug.Log(result);
            callback?.Invoke(result);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ItemsApi.GetCollectionItemBalances: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }

    static void GetCollections(Action<GetCollections200ResponseInner> callback)
    {
        var apiInstance = new ItemsApi(Configuration.Default);

        try
        {
            // Get collections
            List<GetCollections200ResponseInner> result = apiInstance.GetCollections(game_PK);
            Debug.Log(result[0]);
            callback?.Invoke(result[0]);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ItemsApi.GetCollections: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }
    }

    static void GetCollectionItems(string collId)
    {
        var apiInstance = new ItemsApi(Configuration.Default);
        var collectionId = collId;

        try
        {
            // Get collection items
            List<System.Object> result = apiInstance.GetCollectionItems(collectionId);
            Debug.Log(result[0]);
            Debug.Log(result[1]);
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling ItemsApi.GetCollectionItems: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
        }

    }

    #endregion

    #region Utilities

    static string EncodeString(string str)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(str);
        string res = Convert.ToBase64String(bytesToEncode);
        return res;
    }

    static string DecodeString(string str)
    {
        byte[] decodedBytes = Convert.FromBase64String(str);
        string res = Encoding.UTF8.GetString(decodedBytes);
        return res;
    }

    #endregion

    #region Currency

    public static string GetCurrencyBalance()
    {
        if(StaticPlayerData.CurrencyKey == "")
            StaticPlayerData.CurrencyKey = GetCurrencies()[1]?.Id;

        var apiInstance = new CurrenciesApi(Configuration.Default);
        var currencyId = StaticPlayerData.CurrencyKey;  // string | Any currency id within the MetaFab ecosystem.
        var walletId = StaticPlayerData.walletID;  // string | Any wallet id within the MetaFab ecosystem. (optional) 

        try
        {
            // Get currency balance
            decimal result = apiInstance.GetCurrencyBalance(currencyId,walletId: walletId);
            Debug.Log(result);
            return result.ToString();
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling CurrenciesApi.GetCurrencyBalance: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            return "";
        }

    }

    static List<GetCurrencies200ResponseInner> GetCurrencies()
    {
        var apiInstance = new CurrenciesApi(Configuration.Default);
        try
        {
            // Get currencies
            List<GetCurrencies200ResponseInner> result = apiInstance.GetCurrencies(game_PK);
            Debug.Log(result);
            return result;

        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling CurrenciesApi.GetCurrencies: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            return null;

        }
    }

    public static void MintCurrency(int amount = 1,Action OnSuccessCallback=null,Action FailureCallback=null)
    {
        var apiInstance = new CurrenciesApi(Configuration.Default);
        var currencyId = StaticPlayerData.CurrencyKey;  // string | Any currency id within the MetaFab ecosystem.
        var xAuthorization = game_SK;  // string | The `secretKey` of the authenticating game.
        var xPassword = game_ps;  // string | The password of the authenticating game. Required to decrypt and perform blockchain transactions with the game primary wallet.
        var mintCurrencyRequest = new MintCurrencyRequest(amount,walletId: StaticPlayerData.walletID); // MintCurrencyRequest | 

        try
        {
            // Mint currency
            TransactionModel result = apiInstance.MintCurrency(currencyId, xAuthorization, xPassword, mintCurrencyRequest);
            Debug.Log(result);
            OnSuccessCallback?.Invoke();
        }
        catch (ApiException e)
        {
            Debug.Log("Exception when calling CurrenciesApi.MintCurrency: " + e.Message);
            Debug.Log("Status Code: " + e.ErrorCode);
            Debug.Log(e.StackTrace);
            FailureCallback?.Invoke();
        }

    }



    #endregion

}

#region Static PlayerData
public static class StaticPlayerData
{
    public static string walletID = "";
    public static string playerID = "";
    public static string walletAddress = "";
    public static string CurrencyKey = "";
    public static string EquipedWeapon = "";
}
#endregion

#region playerData
public class ProtectedPlayerData
{
    public string WeaponEquipedID;
    public string Score;
}

public struct CollectionItem
{
    public int ID;
    public int count;
}

#endregion