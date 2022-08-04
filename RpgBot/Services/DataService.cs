using RpgBot.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace RpgBot.Data
{
    internal class DataService
    {
        public static List<string> SpellsOsric { get; set; } = new List<string>();
        public static List<string> SpellsAdd { get; set; } = new List<string>();
        public static List<GemValue> GemValues { get; set; } = new List<GemValue>();
        public static List<double> GemValuesExceptional { get; set; } = new List<double>();
        public static List<double> GemValuesPoor { get; set; } = new List<double>();
        public static List<GemProperty> GemProperties { get; set; } = new List<GemProperty>();

        public void LoadSpellText()
        {
            string spellText;

            // load osric spells
            spellText = "";
            using (var fs = File.OpenRead("Data\\osric-spells.txt"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                spellText = sr.ReadToEnd();
            var osricResult = Regex.Split(spellText, @"(?=====.*?====)");

            for (int i = 0; i < osricResult.Length; i++)
            {
                SpellsOsric.Add(osricResult[i]);
            }

            // load ad&d spells
            spellText = "";
            using (var fs = File.OpenRead("Data\\add-spells.txt"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                spellText = sr.ReadToEnd();
            var addResult = Regex.Split(spellText, "====.*\n");

            for (int i = 0; i < addResult.Length; i++)
            {
                SpellsAdd.Add(addResult[i]);
            }
        }

        public void LoadGemstoneData()
        {
            GemValues.Add(new GemValue() { DiceScore = 25, BaseValue = 10, Description = "Ornamental Stone", Size = "very small" });
            GemValues.Add(new GemValue() { DiceScore = 50, BaseValue = 50, Description = "Semi-precious Stone", Size = "small" });
            GemValues.Add(new GemValue() { DiceScore = 70, BaseValue = 100, Description = "Fancy Stone", Size = "average" });
            GemValues.Add(new GemValue() { DiceScore = 90, BaseValue = 500, Description = "Fancy Stone (Precious)", Size = "large" });
            GemValues.Add(new GemValue() { DiceScore = 99, BaseValue = 1000, Description = "Gem Stone", Size = "very large" });
            GemValues.Add(new GemValue() { DiceScore = 100, BaseValue = 5000, Description = "Gem Stone (Jewels)", Size = "huge" });

            GemValuesExceptional.Add(5000);
            GemValuesExceptional.Add(10000);
            GemValuesExceptional.Add(25000);
            GemValuesExceptional.Add(50000);
            GemValuesExceptional.Add(100000);
            GemValuesExceptional.Add(250000);
            GemValuesExceptional.Add(500000);
            GemValuesExceptional.Add(1000000);

            GemValuesPoor.Add(10);
            GemValuesPoor.Add(5);
            GemValuesPoor.Add(1);
            GemValuesPoor.Add(.5);
            GemValuesPoor.Add(.25);
            GemValuesPoor.Add(.05);

            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Azurite: mottled deep blue" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Banded Agate: striped brown and blue and white and reddish" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Blue Quartz: pale blue" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Eye Agate: circles of gray, white, brown, blue and/or green" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Hematite: gray-black" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Lapis Lazuli: light and dark blue with yellow flecks" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Malachite: striated light and dark green" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Moss Agate: pink or yellow-white with grayish or greenish 'moss' markings" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Obsidian: black" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Rhodochrosite: light pink" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Tiger Eye: rich brown with golden center under-hue" });
            GemProperties.Add(new GemProperty() { Type = "Ornamental Stone", BaseValue = 10, NameDescription = "Turquoise: light blue-green" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Bloodstone: dark gray with red flecks" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Carnelian: orange to reddish brown (also called Sard)" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Chalcedony: white" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Chrysoprase: apple green to emerald green" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Citrine: pale yellow brown" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Jasper: blue, black to brown" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Moonstone: white with pale blue glow" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Onyx: bands of black and white or pure black or white" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Rock Crystal: clear" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Sardonyx: bands of sard (red) and onyx (white) or sard" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Smoky Quartz: gray, yellow, or blue (Cairngorm), all light" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Star Rose Quartz: translucent rosy stone with white 'star' center" });
            GemProperties.Add(new GemProperty() { Type = "Semi-precious Stone", BaseValue = 50, NameDescription = "Zircon: clear pale blue-green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Amber: watery gold to rich gold" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Alexandrite: dark green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Amethyst: deep purple" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Chrysoberyl: yellow green to green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Coral: crimson" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Garnet: red or brown-green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Jade: light green, deep green, green and white, white" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Jet: deep black" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Pearl: lustrous white, yellowish, pinkish, etc." });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Spinel: red, red-brown, or deep green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone", BaseValue = 100, NameDescription = "Tourmaline: green pale, blue pale, brown pale, or reddish pale" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Aquamarine: pale blue green" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Pearl: pure black (the most prized)" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Garnet: violet (the most prized)" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Peridot: rich olive green (Chrysolite)" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Spinel: very deep blue (the most prized)" });
            GemProperties.Add(new GemProperty() { Type = "Fancy Stone (Precious)", BaseValue = 500, NameDescription = "Topaz: golden yellow" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Black Opal: dark green with black mottling and golden flecks" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Emerald: deep bright green" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Fire Opal: fiery red" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Opal: pale blue with green and golden mottling" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Oriental Amethyst: rich purple (Corundum)" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Oriental Topaz: fiery yellow (Corundum)" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Sapphire: clear to medium blue (Corundum)" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Star Ruby: translucent ruby with white 'star' center" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone", BaseValue = 1000, NameDescription = "Star Sapphire: translucent sapphire with white 'star' center" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone (Jewel)", BaseValue = 5000, NameDescription = "Black Sapphire: lustrous black with glowing highlights" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone (Jewel)", BaseValue = 5000, NameDescription = "Diamond: clear blue-white with lesser stones clear white or pale tints" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone (Jewel)", BaseValue = 5000, NameDescription = "Jacinth: fiery orange (Corundum)" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone (Jewel)", BaseValue = 5000, NameDescription = "Oriental Emerald: clear bright green (Corundum)" });
            GemProperties.Add(new GemProperty() { Type = "Gem Stone (Jewel)", BaseValue = 5000, NameDescription = "Ruby: clear red to deep crimson (Corundum)" });
        }
    }
}
