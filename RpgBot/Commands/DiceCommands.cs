using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using RpgBot.Data;
using RpgBot.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RpgBot.Commands
{
    internal class DiceCommands : BaseCommandModule
    {
        //readonly Random rnd = new Random();

        [Command("roll"), Aliases("r"), Description("Rolls some dice based on {diceCommand} (4d6|4d6+3|4d6K3) - max 100 dice")]
        public async Task Roll(CommandContext ctx, string diceCommand)
        {
            List<Emoji> Emojis = DataService.Emojis;

            //{
            //    new Emoji(1031015488425304064, "d4", $"https://cdn.discordapp.com/emojis/{1031015488425304064}.png"),
            //    new Emoji(1031017374935175178, "d6", $"https://cdn.discordapp.com/emojis/{1031017374935175178}.png"),
            //    new Emoji(1031017375912443975, "d8", $"https://cdn.discordapp.com/emojis/{1031017375912443975}.png"),
            //    new Emoji(1031017376885526578, "d10", $"https://cdn.discordapp.com/emojis/{1031017376885526578}.png"),
            //    new Emoji(1031017377699221504, "d12", $"https://cdn.discordapp.com/emojis/{1031017377699221504}.png"),
            //    new Emoji(1031017378710044712, "d20", $"https://cdn.discordapp.com/emojis/{1031017378710044712}.png"),
            //    new Emoji(1031017376885526578, "d100", $"https://cdn.discordapp.com/emojis/{1031017376885526578}.png")
            //};


            //var dFourEmoji = "https://cdn.discordapp.com/emojis/1031015488425304064.png";
            //var dSixEmoji = "https://cdn.discordapp.com/emojis/1031017374935175178.png";
            //var dEightEmoji = "https://cdn.discordapp.com/emojis/1031017375912443975.png";
            //var dTenEmoji = "https://cdn.discordapp.com/emojis/1031017376885526578.png";
            //var dTwelveEmoji = "https://cdn.discordapp.com/emojis/1031017377699221504.png";
            //var dTwentyEmoji = "https://cdn.discordapp.com/emojis/1031017378710044712.png";

            int diceQuantity;
            int diceType;
            string diceOperator = string.Empty;
            int diceModifier;
            string diceRolled = string.Empty;
            string diceToKeep = "All";
            Emoji diceTypeEmoji;
            string diceTypeEmojiCode = string.Empty;

            char[] mathOperators = { '+', '-', '*' };

            if (mathOperators.Any(c => diceCommand.Contains(c)))
            {
                foreach (char c in diceCommand)
                {
                    if (mathOperators.Contains(c))
                    {
                        diceOperator = c.ToString();
                    }
                }

                string[] dice = diceCommand.Split(mathOperators);

                diceQuantity = int.Parse(dice[0].ToUpper().Split("D")[0]);

                if (diceQuantity > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceType = int.Parse(dice[0].ToUpper().Split("D")[1]); // e.g. 6
                diceModifier = int.Parse(dice[1]);
                diceRolled = dice[0]; // e.g. 4d6
                diceTypeEmoji = Emojis.Where(x => x.Name == $"d{diceType}").FirstOrDefault();
                diceTypeEmojiCode = $"<:{diceTypeEmoji.Name}:{diceTypeEmoji.Id}>";

                //string diceRolled = $"{diceQuantity} d {diceType}";

                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                for (int i = 0; i < diceQuantity; i++)
                {
                    diceValues.Add(RandomNumberGenerator.GetInt32(1, diceType + 1));
                }

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"**{item}** ";
                    diceValuesTotal += item;
                }

                if (diceOperator == "+")
                {
                    diceValuesTotal += diceModifier;
                }
                else if (diceOperator == "-")
                {
                    diceValuesTotal -= diceModifier;
                }
                else if (diceOperator == "*")
                {
                    diceValuesTotal *= diceModifier;
                }

                // strip last comma to make it more pretty
                diceValuesResult = diceValuesResult.Remove(diceValuesResult.Length - 1, 1);

                await ctx.RespondAsync($"{diceTypeEmojiCode} **{ctx.Member.Username}** rolls **{diceValuesTotal}** {diceTypeEmojiCode}\r *{diceRolled}* [{diceValuesResult}] *modified by {diceOperator}{diceModifier}*");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceOperator: {diceOperator} diceModifier: {diceModifier} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
            else if (diceCommand.ToUpper().Contains("K"))
            {
                string[] dice = diceCommand.ToUpper().Split("D");

                diceQuantity = int.Parse(dice[0]); // e.g. 4

                if (diceQuantity > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceType = int.Parse(dice[1].ToUpper().Split("K")[0]); // e.g. 6
                diceRolled = $"{dice[0]}d{diceType}"; // e.g. 4d6
                diceToKeep = dice[1].ToUpper().Split("K")[1]; // e.g. 3
                diceTypeEmoji = Emojis.Where(x => x.Name == $"d{diceType}").FirstOrDefault();
                diceTypeEmojiCode = $"<:{diceTypeEmoji.Name}:{diceTypeEmoji.Id}>";

                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                for (int i = 0; i < diceQuantity; i++)
                {
                    diceValues.Add(RandomNumberGenerator.GetInt32(1, diceType + 1));
                }

                diceValues = diceValues.OrderBy(x => x).ToList();

                List<int> diceValuesBeforeDrop = new List<int>(diceValues);
                string diceValuesBeforeDropResult = string.Empty;

                foreach (int item in diceValuesBeforeDrop)
                {
                    diceValuesBeforeDropResult += $"**{item}** ";
                    //diceValuesTotal += item;
                }

                diceValues.RemoveRange(0, diceQuantity - int.Parse(diceToKeep));

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"**{item}** ";
                    diceValuesTotal += item;
                }

                // strip last comma to make it more pretty
                diceValuesResult = diceValuesResult.Remove(diceValuesResult.Length - 1, 1);

                await ctx.RespondAsync($"{diceTypeEmojiCode} **{ctx.Member.Username}** rolls **{diceValuesTotal}** {diceTypeEmojiCode}\r *{diceRolled}* [{diceValuesBeforeDropResult}] *keep highest {diceToKeep}* [{diceValuesResult}]");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceToKeep: {diceToKeep} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
            else
            {
                string[] dice = diceCommand.ToUpper().Split("D");

                diceQuantity = int.Parse(dice[0]);

                if (diceQuantity > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceType = int.Parse(dice[1]); // e.g. 6
                //diceRolled = dice[0]; // e.g. 4d6
                diceRolled = $"{dice[0]}d{diceType}"; // e.g. 4d6
                diceTypeEmoji = Emojis.Where(x => x.Name == $"d{diceType}").FirstOrDefault();
                diceTypeEmojiCode = $"<:{diceTypeEmoji.Name}:{diceTypeEmoji.Id}>";

                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                for (int i = 0; i < diceQuantity; i++)
                {
                    diceValues.Add(RandomNumberGenerator.GetInt32(1, diceType + 1));
                }

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"**{item}** ";
                    diceValuesTotal += item;
                }

                // strip last comma to make it more pretty
                diceValuesResult = diceValuesResult.Remove(diceValuesResult.Length - 1, 1);

                //await ctx.RespondAsync($"{diceTypeEmojiCode} {ctx.Member.Username} rolls... {diceValuesTotal}! ...with these dice: [{diceValuesResult}] {diceTypeEmojiCode}");
                await ctx.RespondAsync($"{diceTypeEmojiCode} **{ctx.Member.Username}** rolls **{diceValuesTotal}** {diceTypeEmojiCode}\r *{diceRolled}* [{diceValuesResult}]");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
        }

        [Command("char"), Aliases("ch"), Description("Generates a set of ability scores for an AD&D character via {abilityScoreMethod} (I|II)")]
        public async Task Char(CommandContext ctx, string abilityScoreMethod)
        {
            string diceCommand = string.Empty;
            int diceNumber;
            int diceType;
            string diceToKeep = "All";
            string abilityScoresResult = string.Empty;

            List<int> abilityScoreResults = new List<int>();

            if (abilityScoreMethod == "I")
            {
                diceCommand = "4D6K3";

                string[] dice = diceCommand.ToUpper().Split("D");

                diceNumber = int.Parse(dice[0]);
                diceToKeep = dice[1].ToUpper().Split("K")[1];
                diceType = int.Parse(dice[1].ToUpper().Split("K")[0]);

                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                for (int i = 0; i < 6; i++)
                {
                    // roll the dice sets
                    for (int j = 0; j < diceNumber; j++)
                    {
                        diceValues.Add(RandomNumberGenerator.GetInt32(1, diceType + 1));
                    }

                    diceValues = diceValues.OrderBy(x => x).ToList();
                    diceValues.RemoveRange(0, diceNumber - int.Parse(diceToKeep));

                    foreach (int item in diceValues)
                    {
                        diceValuesTotal += item;
                    }

                    abilityScoreResults.Add(diceValuesTotal);
                    diceValuesTotal = 0;

                    diceValues.Clear();
                }
            }
            else if (abilityScoreMethod == "II")
            {
                diceCommand = "3D6";

                string[] dice = diceCommand.ToUpper().Split("D");

                diceNumber = int.Parse(dice[0]);
                diceType = int.Parse(dice[1]);
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                for (int i = 0; i < 12; i++)
                {
                    // roll the dice sets
                    for (int j = 0; j < diceNumber; j++)
                    {
                        diceValues.Add(RandomNumberGenerator.GetInt32(1, diceType + 1));
                    }

                    foreach (int item in diceValues)
                    {
                        diceValuesTotal += item;
                    }

                    abilityScoreResults.Add(diceValuesTotal);
                    diceValuesTotal = 0;

                    diceValues.Clear();
                }

                abilityScoreResults = abilityScoreResults.OrderBy(x => x).ToList();
                abilityScoreResults.RemoveRange(0, 6);
            }

            foreach (int item in abilityScoreResults)
            {
                abilityScoresResult += $"{item}, ";
            }

            string abilityScoreMethodText = string.Empty;

            if (abilityScoreMethod == "I")
            {
                abilityScoreMethodText = "rolling 4d6K3 six times";
            }
            else if (abilityScoreMethod == "II")
            {
                abilityScoreMethodText = "rolling 3D6 twelve times and keeping the highest 6";
            }

            // strip last comma to make it more pretty
            abilityScoresResult = abilityScoresResult.Remove(abilityScoresResult.Length - 1, 1);
            await ctx.RespondAsync($"A character has been generated for {ctx.Member.Username} by {abilityScoreMethodText}, resulting in the following ability scores: {abilityScoresResult}");
            //await ctx.RespondAsync($"abilityScoresResult: {abilityScoresResult}");
        }

        [Command("splash"), Aliases("spl"), Description("Calculates the direction, distance and splash damage from misses with a grenade-like missile, by {missileType} (acid|oil|water)")]
        public async Task Splash(CommandContext ctx, string missileType)
        {
            int distanceFeet = RandomNumberGenerator.GetInt32(1, 7);

            Dictionary<int, string> directions = new Dictionary<int, string>
            {
                { 1, "long right" },
                { 2, "right" },
                { 3, "short right" },
                { 4, "short (before)" },
                { 5, "short left" },
                { 6, "left" },
                { 7, "long left" },
                { 8, "long (over)" }
            };

            int directionResult = RandomNumberGenerator.GetInt32(1, 9);

            string direction = directions.ElementAt(directionResult).Value;

            int splashDamage = 1;

            if (missileType == "water")
            {
                splashDamage = 2;
            }
            else if (missileType == "oil")
            {
                splashDamage = RandomNumberGenerator.GetInt32(1, 4);
            }

            await ctx.RespondAsync($"The thrown {missileType} landed {distanceFeet} feet {direction} and did {splashDamage} damage.");
        }
    }
}
