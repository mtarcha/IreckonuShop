using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using IreckonuShop.Domain;

namespace IreckonuShop.BusinessLogic.Services
{
    public class DutchParser : ILocalizedStringParser
    {
        private readonly Regex _deliveryRangeRegex = new Regex(@"(?<from>^\d+)-(?<to>\d+) werkdagen");

        private Dictionary<string, Color> _nameToColorMap = new Dictionary<string, Color>
        {
            {"blauw", Color.Blue },
            {"oranje", Color.Orange },
            {"groen", Color.Green },
            {"geel", Color.Yellow },
            {"wit", Color.White },
            {"zwart", Color.Black },
            {"rood", Color.Red },
            {"paars", Color.Purple },
            {"roze", Color.Pink },
            {"bruin", Color.Brown },
        };

        private Dictionary<string, TargetClient> _targetNameToTargetClient = new Dictionary<string, TargetClient>
        {
            {"boy", TargetClient.Boy },
            {"girl", TargetClient.Girl },
            {"baby", TargetClient.Baby },
            {"NOINDEX", TargetClient.Unknown },
        };

        public Color ParseColor(string colorName)
        {
            if (!_nameToColorMap.TryGetValue(colorName.ToLower(), out var color))
            {
                throw new ArgumentException("Unknown color name");
            }

            return color;
        }

        public DeliveryRange ParseDeliveryRange(string deliveryRange)
        {
            var match = _deliveryRangeRegex.Match(deliveryRange);
            if (match.Success)
            {
                var groups = match.Groups;
                var from = int.Parse(groups["from"].Value);
                var to = int.Parse(groups["to"].Value);

                return new DeliveryRange(TimeSpan.FromDays(from), TimeSpan.FromDays(to));
            }

            throw new ArgumentException("Incorrect delivery range format");
        }

        public TargetClient ParseTargetClient(string targetClient)
        {
            if (!_targetNameToTargetClient.TryGetValue(targetClient.ToLower(), out var client))
            {
                throw new ArgumentException("Unknown target client name");
            }

            return client;
        }
    }
}