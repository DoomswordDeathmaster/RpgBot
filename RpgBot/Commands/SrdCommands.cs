using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using RpgBot.Data;
using RpgBot.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace RpgBot.Commands
{
    internal class SrdCommands : BaseCommandModule
    {
        //readonly Random rnd = new Random();

        [Command("spell"), Aliases("sp"), Description("Returns AD&D spells that begin with {spellName}")]
        public async Task Spell(CommandContext ctx, string spellName)
        {
            string spell = DataService.SpellsAdd.Where(x => x.StartsWith($"{spellName}")).FirstOrDefault();
            await ctx.RespondAsync(spell);
        }

        [Command("spellosric"), Aliases("spo"), Description("Returns OSRIC spells that begin with {spellName}")]
        public async Task SpellOsric(CommandContext ctx, string spellName)
        {
            string spell = DataService.SpellsOsric.Where(x => x.Contains($"==== {spellName}")).FirstOrDefault();
            await ctx.RespondAsync(spell);
        }

        [Command("gemstones"), Aliases("gs"), Description("Calculates the values of gemstones for the desired {numberOfGemstones} - 25 max")]
        public async Task Gemstones(CommandContext ctx, int numberOfGemstones)
        {
            if (numberOfGemstones > 25)
            {
                await ctx.RespondAsync($"{ctx.Member.Username}, please limit the gemstone quantity to 25 or less");
                return;
            }

            List<GemValue> generatedGemValues = new List<GemValue>();

            for (int i = 0; i < numberOfGemstones; i++)
            {
                int percentileResult = RandomNumberGenerator.GetInt32(1, 101);

                GemValue gemValue = DataService.GemValues.Where(x => x.DiceScore >= percentileResult).First();

                generatedGemValues.Add(gemValue);
            }

            string gemValueResults = string.Empty;

            foreach (var item in generatedGemValues)
            {
                double gemBaseValue = item.BaseValue;

                var gemPropertiesPossible = DataService.GemProperties.Where(x => x.BaseValue == gemBaseValue).ToList();
                int gemPropertiesIndex = RandomNumberGenerator.GetInt32(gemPropertiesPossible.Count());

                var gemProperty = gemPropertiesPossible[gemPropertiesIndex];

                int increaseDecreaseDieResult = RandomNumberGenerator.GetInt32(1, 11);

                if (increaseDecreaseDieResult == 2)
                {
                    gemBaseValue *= 2;
                }
                else if (increaseDecreaseDieResult == 3)
                {
                    int increaseD6Result = RandomNumberGenerator.GetInt32(1, 7);
                    double increasePercentage = (double)(increaseD6Result * 10) / 100;

                    double gemIncreaseValue = gemBaseValue * increasePercentage;
                    gemBaseValue += (int)gemIncreaseValue;
                }
                else if (increaseDecreaseDieResult == 9)
                {
                    int decreaseD4Result = RandomNumberGenerator.GetInt32(1, 5);
                    double decreasePercentage = (double)(decreaseD4Result * 10) / 100;

                    double gemDecreaseValue = gemBaseValue * decreasePercentage;
                    gemBaseValue -= (int)gemDecreaseValue;
                }
                else if (increaseDecreaseDieResult == 1)
                {
                    int i = 0;
                    bool breakFlag = false;

                    while (i < 7 && breakFlag == false)
                    {
                        if (gemBaseValue >= 5000)
                        {
                            int newGemValueExceptionalListIndex = DataService.GemValuesExceptional.IndexOf(gemBaseValue) + 1;
                            double gemValue = DataService.GemValuesExceptional[newGemValueExceptionalListIndex];

                            gemBaseValue = gemValue;
                        }
                        else
                        {
                            int newGemValueListIndex = DataService.GemValues.IndexOf(item) + 1;
                            GemValue gemValue = DataService.GemValues[newGemValueListIndex];

                            gemBaseValue = gemValue.BaseValue;
                        }

                        increaseDecreaseDieResult = RandomNumberGenerator.GetInt32(1, 11);

                        if (increaseDecreaseDieResult == 2)
                        {
                            gemBaseValue *= 2;

                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult == 3)
                        {
                            int increaseD6Result = RandomNumberGenerator.GetInt32(1, 7);
                            double increasePercentage = (double)(increaseD6Result * 10) / 100;

                            double gemIncreaseValue = gemBaseValue * increasePercentage;
                            gemBaseValue += (int)gemIncreaseValue;

                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult >= 4)
                        {
                            breakFlag = true;
                        }

                        i++;
                    }
                }
                else if (increaseDecreaseDieResult == 10)
                {
                    int i = 0;
                    bool breakFlag = false;

                    while (i < 5 && breakFlag == false)
                    {
                        if (gemBaseValue <= 10)
                        {
                            int newGemValuePoorlListIndex = DataService.GemValuesPoor.IndexOf(gemBaseValue) + 1;
                            double gemValue = DataService.GemValuesPoor[newGemValuePoorlListIndex];

                            gemBaseValue = gemValue;
                        }
                        else
                        {
                            int newGemValueListIndex = DataService.GemValues.IndexOf(item) - 1;
                            GemValue gemValue = DataService.GemValues[newGemValueListIndex];

                            gemBaseValue = gemValue.BaseValue;
                        }

                        increaseDecreaseDieResult = RandomNumberGenerator.GetInt32(1, 11);

                        if (increaseDecreaseDieResult == 1)
                        {
                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult == 2)
                        {
                            gemBaseValue *= 2;

                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult == 3)
                        {
                            int increaseD6Result = RandomNumberGenerator.GetInt32(1, 7);
                            double increasePercentage = (double)(increaseD6Result * 10) / 100;

                            double gemIncreaseValue = gemBaseValue * increasePercentage;
                            gemBaseValue += (int)gemIncreaseValue;

                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult >= 4 && increaseDecreaseDieResult <= 8)
                        {
                            breakFlag = true;
                        }
                        else if (increaseDecreaseDieResult == 9)
                        {
                            int decreaseD4Result = RandomNumberGenerator.GetInt32(1, 5);
                            double decreasePercentage = (double)(decreaseD4Result * 10) / 100;

                            double gemDecreaseValue = gemBaseValue * decreasePercentage;
                            gemBaseValue -= (int)gemDecreaseValue;

                            breakFlag = true;
                        }

                        i++;
                    }
                }

                gemValueResults += $"{gemBaseValue} GP {gemProperty.Type} - {gemProperty.NameDescription}\r\n";
            }

            await ctx.RespondAsync(gemValueResults);
        }
    }
}
