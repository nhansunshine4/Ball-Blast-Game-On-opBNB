using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using TMPro;

public class BlockchainManager : MonoBehaviour
{
    public string Address { get; private set; }

    public Button playBtn;
    public TextMeshProUGUI bestLevelTxt;

    public Button nftBtn;
    public TextMeshProUGUI nftBtnTxt;

    private string NFTAddress = "0x3Ce1dC020e7160013bE9c24bE90E0e8199F0d752";

    public GameoverDialog gameoverDialog;

    public Button replayBtn;
    public TextMeshProUGUI replayBtnTxt;
    public Button homeBtn;
    public TextMeshProUGUI homeBtnTxt;

    private void Start()
    {
        playBtn.gameObject.SetActive(false);
        bestLevelTxt.gameObject.SetActive(false);
        nftBtn.gameObject.SetActive(false);

        if (gameoverDialog == null)
        {
            GameObject gameoverDialogObject = GameObject.Find("GameoverDialog");
            if (gameoverDialogObject != null)
            {
                gameoverDialog = gameoverDialogObject.GetComponent<GameoverDialog>();
            }
        }
    }

    public async void Login()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        //string testAddress = "0x4C6F5f4e21840f1e103fF8791AC70b4Ff1AD59f9";
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddress);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            nftBtn.gameObject.SetActive(true);
            playBtn.gameObject.SetActive(false);
        }
        else
        {
            nftBtn.gameObject.SetActive(false);
            playBtn.gameObject.SetActive(true);
            ShowBestLevel();
        }
    }

    public async void ClaimNFT()
    {
        nftBtn.interactable = false;
        nftBtnTxt.text = "Claiming!";
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddress);
        await contract.ERC721.ClaimTo(Address,1);
        nftBtn.interactable = true;
        nftBtnTxt.text = "Claim NFT";
        nftBtn.gameObject.SetActive(false);
        playBtn.gameObject.SetActive(true);
    }

    public async void PlayGame()
    {
        Debug.Log("PlayGame");
        playBtn.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            "0x74CdE855D7Ea63A4FAf896bfb877CBD486aCEe05",
            "[{\"type\":\"event\",\"name\":\"highestLevelEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newSaving\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"txCountEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newCount\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"getHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"savingLevel\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"setTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );

        await contract.Write("setTxCount", Address);

        playBtn.interactable = true;
        //Play Game
        GameManager.Ins.PlayGame();
    }

    public void TestPlayGame() {
        GameManager.Ins.PlayGame();
    }

    public async void SavingBestLevelOntoChainReplay() {
        replayBtn.interactable = false;
        replayBtnTxt.text = "Loading...";
        homeBtn.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            "0x74CdE855D7Ea63A4FAf896bfb877CBD486aCEe05",
            "[{\"type\":\"event\",\"name\":\"highestLevelEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newSaving\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"txCountEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newCount\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"getHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"savingLevel\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"setTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );

        int bestLevelOnChain = await contract.Read<int>("getHighestLevel", Address);
        if (Prefs.bestLevel > 0 && bestLevelOnChain < Prefs.bestLevel)
        {
            await contract.Write("setHighestLevel", Address, Prefs.bestLevel);
        }

        if (gameoverDialog != null)
        {
            gameoverDialog.Replay();
        }
        else
        {
            Debug.LogWarning("GameoverDialog");
        }

        replayBtn.interactable = true;
        replayBtnTxt.text = "Replay";
        homeBtn.interactable = true;
    }

    public async void SavingBestLevelOntoChainHome()
    {
        homeBtn.interactable = false;
        homeBtnTxt.text = "Loading...";
        replayBtn.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            "0x74CdE855D7Ea63A4FAf896bfb877CBD486aCEe05",
            "[{\"type\":\"event\",\"name\":\"highestLevelEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newSaving\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"txCountEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newCount\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"getHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"savingLevel\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"setTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );

        int bestLevelOnChain = await contract.Read<int>("getHighestLevel", Address);
        if (Prefs.bestLevel > 0 && bestLevelOnChain < Prefs.bestLevel)
        {
            await contract.Write("setHighestLevel", Address, Prefs.bestLevel);
        }

        if (gameoverDialog != null)
        {
            gameoverDialog.BackHome();
        }
        else
        {
            Debug.LogWarning("GameoverDialog");
        }

        homeBtn.interactable = true;
        homeBtnTxt.text = "Home";
        replayBtn.interactable = true;

    }

    public async void ShowBestLevel() {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(
            "0x74CdE855D7Ea63A4FAf896bfb877CBD486aCEe05",
            "[{\"type\":\"event\",\"name\":\"highestLevelEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newSaving\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"event\",\"name\":\"txCountEvent\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"indexed\":true,\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"newCount\",\"indexed\":false,\"internalType\":\"uint256\"}],\"outputs\":[],\"anonymous\":false},{\"type\":\"function\",\"name\":\"getHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"getTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[{\"type\":\"uint256\",\"name\":\"\",\"internalType\":\"uint256\"}],\"stateMutability\":\"view\"},{\"type\":\"function\",\"name\":\"setHighestLevel\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"},{\"type\":\"uint256\",\"name\":\"savingLevel\",\"internalType\":\"uint256\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"},{\"type\":\"function\",\"name\":\"setTxCount\",\"inputs\":[{\"type\":\"address\",\"name\":\"player\",\"internalType\":\"address\"}],\"outputs\":[],\"stateMutability\":\"nonpayable\"}]"
            );

        int bestLevelOnChain = await contract.Read<int>("getHighestLevel", Address);
        bestLevelTxt.text = "Best Level: " + bestLevelOnChain.ToString();
        bestLevelTxt.gameObject.SetActive(true);
    }
}
