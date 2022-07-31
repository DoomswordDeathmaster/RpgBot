using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RpgBot.Commands
{
    internal class DiceCommands : BaseCommandModule
    {
        [Command("roll"), Description("Rolls some dice based on {diceCommand} (4d6|4d6+3|4d6K3) - max 100 dice")]
        public async Task Roll(CommandContext ctx, string diceCommand)
        {
            var emoji = DiscordEmoji.FromName(ctx.Client, ":game_die:");

            int diceNumber;
            int diceType;
            string diceOperator = "";
            int diceModifier;
            string diceToKeep = "All";

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

                diceNumber = int.Parse(dice[0].ToUpper().Split("D")[0]);

                if (diceNumber > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceType = int.Parse(dice[0].ToUpper().Split("D")[1]);
                diceModifier = int.Parse(dice[1]);
                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                Random rnd = new Random();

                for (int i = 0; i < diceNumber; i++)
                {
                    diceValues.Add(rnd.Next(1, diceType + 1));
                }

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"{item},";
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

                await ctx.RespondAsync($"{emoji} {ctx.Member.Username} rolls... {diceValuesTotal}! ...with these dice: [{diceValuesResult}] modified {diceOperator}{diceModifier} {emoji}");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceOperator: {diceOperator} diceModifier: {diceModifier} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
            else if (diceCommand.ToUpper().Contains("K"))
            {
                string[] dice = diceCommand.ToUpper().Split("D");

                diceNumber = int.Parse(dice[0]);

                if (diceNumber > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceToKeep = dice[1].ToUpper().Split("K")[1];
                diceType = int.Parse(dice[1].ToUpper().Split("K")[0]);
                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                Random rnd = new Random();

                for (int i = 0; i < diceNumber; i++)
                {
                    diceValues.Add(rnd.Next(1, diceType + 1));
                }

                diceValues = diceValues.OrderBy(x => x).ToList();
                diceValues.RemoveRange(0, diceNumber - int.Parse(diceToKeep));

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"{item},";
                    diceValuesTotal += item;
                }

                // strip last comma to make it more pretty
                diceValuesResult = diceValuesResult.Remove(diceValuesResult.Length - 1, 1);

                await ctx.RespondAsync($"{emoji} {ctx.Member.Username} rolls... {diceValuesTotal}! ...with these dice: [{diceValuesResult}] {emoji}");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceToKeep: {diceToKeep} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
            else
            {
                string[] dice = diceCommand.ToUpper().Split("D");

                diceNumber = int.Parse(dice[0]);

                if (diceNumber > 100)
                {
                    await ctx.RespondAsync($"{ctx.Member.Username}, please roll no more than 100 dice at a time");
                    return;
                }

                diceType = int.Parse(dice[1]);
                string diceValuesResult = string.Empty;
                int diceValuesTotal = 0;

                List<int> diceValues = new List<int>();

                Random rnd = new Random();

                for (int i = 0; i < diceNumber; i++)
                {
                    diceValues.Add(rnd.Next(1, diceType + 1));
                }

                foreach (int item in diceValues)
                {
                    diceValuesResult += $"{item},";
                    diceValuesTotal += item;
                }

                // strip last comma to make it more pretty
                diceValuesResult = diceValuesResult.Remove(diceValuesResult.Length - 1, 1);

                await ctx.RespondAsync($"{emoji} {ctx.Member.Username} rolls... {diceValuesTotal}! ...with these dice: [{diceValuesResult}] {emoji}");
                //await ctx.RespondAsync($"diceNumber: {diceNumber} diceType: {diceType} diceValuesResult: {diceValuesResult} diceValuesTotal: {diceValuesTotal}");
            }
        }

        [Command("char"), Description("Generates a set of ability scores for an AD&D character via {abilityScoreMethod} (I|II)")]
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

                Random rnd = new Random();

                for (int i = 0; i < 6; i++)
                {
                    // roll the dice sets
                    for (int j = 0; j < diceNumber; j++)
                    {
                        diceValues.Add(rnd.Next(1, diceType + 1));
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

                Random rnd = new Random();

                for (int i = 0; i < 12; i++)
                {
                    // roll the dice sets
                    for (int j = 0; j < diceNumber; j++)
                    {
                        diceValues.Add(rnd.Next(1, diceType + 1));
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
                abilityScoreMethodText = "rolling 3D6 twelve times and keeping the highest 6 results";
            }

            // strip last comma to make it more pretty
            abilityScoresResult = abilityScoresResult.Remove(abilityScoresResult.Length - 1, 1);
            await ctx.RespondAsync($"A character has been generated for {ctx.Member.Username} by {abilityScoreMethodText}, with the following ability scores: {abilityScoresResult}");
            //await ctx.RespondAsync($"abilityScoresResult: {abilityScoresResult}");
        }

        [Command("splash"), Description("Calculates the direction, distance and splash damage from misses with a grenade-like missile, by {missileType} (acid|oil|water)")]
        public async Task Splash(CommandContext ctx, string missileType)
        {
            Random rnd = new Random();

            int distanceFeet = rnd.Next(1, 7);

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

            int directionResult = rnd.Next(1, 9);

            string direction = directions.ElementAt(directionResult).Value;

            int splashDamage = 1;

            if (missileType == "water")
            {
                splashDamage = 2;
            }
            else if (missileType == "oil")
            {
                splashDamage = rnd.Next(1, 4);
            }

            await ctx.RespondAsync($"The thrown {missileType} landed {distanceFeet} feet {direction} and did {splashDamage} damage.");
        }
    }
}
