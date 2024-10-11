namespace RewardWallet.Models;

[Serializable]
public class Result
{
    private string _errorMsg = "Success";

    public int ErrorCode { get; set; }

    public string ErrorMsg
    {
        get
        {
            if (ErrorCode == 0)
            {
                _errorMsg = "Success";
            }

            return _errorMsg;
        }

        set => _errorMsg = value;
    }

    public bool IsFail => ErrorCode > 0;
};