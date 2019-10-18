using System.Drawing;
using IreckonuShop.Domain;

namespace IreckonuShop.BusinessLogic.Services
{
    public interface ILocalizedStringParser
    {
        Color ParseColor(string colorName);

        DeliveryRange ParseDeliveryRange(string deliveryRange);

        TargetClient ParseTargetClient(string targetClient);
    }
}