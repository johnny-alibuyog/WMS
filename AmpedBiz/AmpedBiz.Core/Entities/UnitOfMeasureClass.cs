using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Extentions;

namespace AmpedBiz.Core.Entities
{
    // Reference: https://docs.oracle.com/cd/A60725_05/html/comnls/us/inv/uomov.htm#c_uomov

    public class UnitOfMeasureClass : Entity<string, UnitOfMeasureClass>
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<UnitOfMeasure> Units { get; protected set; } = new Collection<UnitOfMeasure>();

        public UnitOfMeasureClass() : base(default(string)) { }

        public UnitOfMeasureClass(string id = null, string name = null) : base(id)
        {
            this.Name = name;
            this.Units = new Collection<UnitOfMeasure>();
        }

        public virtual UnitOfMeasureClass WithUnits(IEnumerable<UnitOfMeasure> items)
        {
            var itemsToInsert = items.Except(this.Units).ToList();
            var itemsToUpdate = this.Units.Where(x => items.Contains(x)).ToList();
            var itemsToRemove = this.Units.Except(items).ToList();

            // insert
            foreach (var item in itemsToInsert)
            {
                item.UnitOfMeasureClass = this;
                this.Units.Add(item);
            }

            // update
            foreach (var item in itemsToUpdate)
            {
                var value = items.Single(x => x == item);
                value.UnitOfMeasureClass = this;
                item.SerializeWith(value);
            }

            // delete
            foreach (var item in itemsToRemove)
            {
                item.UnitOfMeasureClass = null;
                Units.Remove(item);
            }

            return this;
        }

        public static UnitOfMeasureClass Quantity = new UnitOfMeasureClass("qty", "Quantity")
            .WithUnits(new Collection<UnitOfMeasure>()
            {
                new UnitOfMeasure("ea", "each", true, 1M),
                new UnitOfMeasure("dz", "dozen", false, 12M),
                new UnitOfMeasure("cs", "case", false),
                new UnitOfMeasure("sk", "sack", false),
            });

        public static UnitOfMeasureClass Weight = new UnitOfMeasureClass("wt", "Weight")
            .WithUnits(new Collection<UnitOfMeasure>()
            {
                new UnitOfMeasure("mg", "milligram", false, (1M/1000000M)),
                new UnitOfMeasure("cg", "centigram", false, (1M/100000M)),
                new UnitOfMeasure("dg", "decigram", false,  (1M/10000M)),
                new UnitOfMeasure("g",  "gram", false, (1M/1000M)),
                new UnitOfMeasure("dag", "dekagram", false, (1M/100M)),
                new UnitOfMeasure("hg", "hectogram", false, (1M/10M)),
                new UnitOfMeasure("kl", "kilogram", true, 1M),
                new UnitOfMeasure("t", "metric ton", false, (1000M)),
            });

        public static UnitOfMeasureClass Volume = new UnitOfMeasureClass("vol", "Volume")
            .WithUnits(new Collection<UnitOfMeasure>()
            {
                new UnitOfMeasure("ml", "milliliter", false, (1M/1000M)),
                new UnitOfMeasure("cl", "centiliter", false, (1M/100M)),
                new UnitOfMeasure("dl", "deciliter", false, (1M/10M)),
                new UnitOfMeasure("l", "liter", true, (1M)),
                new UnitOfMeasure("dal", "dekaliter", false, (10M)),
                new UnitOfMeasure("hl", "hectoliter", false, (100M)),
            });

        public static UnitOfMeasureClass Length = new UnitOfMeasureClass("l", "Length")
            .WithUnits(new Collection<UnitOfMeasure>()
            {
                new UnitOfMeasure("mm", "millimeter", false, (1M/1000M)),
                new UnitOfMeasure("cm", "centimeter", false, (1M/100M)),
                new UnitOfMeasure("dm", "decimeter", false, (1M/10M)),
                new UnitOfMeasure("m", "meter", true, (1M)),
                new UnitOfMeasure("dam", "dekameter", false, (10M)),
                new UnitOfMeasure("hm", "hectometer", false, (100M)),
            });

        public static readonly IEnumerable<UnitOfMeasureClass> All = new Collection<UnitOfMeasureClass>() { Quantity, Weight, Volume, Length };
    }

    /*
    Length
    Metric System
    1 millimeter  =  1/1,000 meter
    1 centimeter  =  1/100 meter
    1 decimeter  =  1/10 meter
    1 meter (basic unit of length)
    1 dekameter  =  10 meters
    1 kilometer  =  1,000 meters

    American and British Units
    1 inch  =  1/36 yard  =  1/12 foot
    1 foot  =  1/3 yard
    1 yard (basic unit of length)
    1 rod  =  5 1/2 yards
    1 furlong  =  220 yards  =  40 rods
    1 mile  =  1,760 yards  =  5,280 feet
    1 fathom  =  6 feet
    1 nautical mile  =  6,076.1 feet

    Conversion Factors
    1 centimeter  =  0.39 inch
    1 inch  =  2.54 centimeters
    1 meter  =  39.37 inches
    1 foot  =  0.305 meter
    1 meter  =  3.28 feet
    1 yard  =  0.914 meter
    1 meter  =  1.094 yards
    1 kilometer  =  0.62 mile
    1 mile  =  1.609 kilometers

    Area
    Metric System
    1 square centimeter  =  1/10,000 square meter
    1 square decimeter  =  1/100 square meter
    1 square meter (basic unit of area)
    1 are  =  100 square meters
    1 hectare  =  10,000 square meters  =  100 ares
    1 square kilometer  =  1,000,000 square meters

    American and British Units
    1 square inch  =  1/1,296 square yard  =  1/144 square foot
    1 square foot  =  1/9 square yard
    1 square yard (basic unit of area)
    1 square rod  =  30 1/4 square yards
    1 acre  =  4,840 square yards  =  160 square rods
    1 square mile  =  3,097,600 square yards  =  640 acres

    Conversion Factors
    1 square centimeter  =  0.155 square inch
    1 square inch  =  6.45 square centimeters
    1 acre  =  0.405 hectare
    1 hectare  =  2.47 acres
    1 square kilometer  =  0.386 square mile
    1 square mile  =  2.59 square kilometers

    Volume and Capacity (Liquid and Dry)
    Metric System
    1 cubic centimeter  =  1/1,000,000 cubic meter
    1 cubic decimeter  =  1/1,000 cubic meter
    1 cubic meter  =  1 stere (basic unit of volume)
    1 milliliter  =  1/1,000 liter  =  1 cubic centimeter
    1 centiliter  =  1/100 liter
    1 deciliter  =  1/10 liter
    1 liter  =  1 cubic decimeter (basic unit of capacity)
    1 dekaliter  =  10 liters
    1 hectoliter  =  100 liters  =  1/10 cubic meter

    American and British Units
    1 cubic inch  =  1/46,656 cubic yard  =  1/1,728 cubic foot
    1 cubic foot  =  1/27 cubic yard
    1 cubic yard (basic unit of volume)
    1 U.S. fluid ounce  =  1/128 U.S. gallon  =  1/16 U.S. pint
    1 British imperial fluid ounce  =  1/160 imperial gallon  =  1/20 imperial pint
    1 pint  =  1/8 gallon  =  1/2 quart
    1 quart  =  1/4 gallon
    1 U.S. gallon (basic unit of liquid capacity in the United States)  =  231 cubic inches
    1 imperial gallon (basic unit of liquid capacity in some Commonwealth nations)  =  277.4 cubic inches
    1 dry pint  =  1/64 bushel  =  1/2 dry quart
    1 dry quart  =  1/32 bushel  =  1/8 peck
    1 peck  =  1/4 bushel
    1 U.S. bushel (basic unit of dry capacity in the United States)  =  2,150.4 cubic inches
    1 imperial bushel (basic unit of dry capacity in some Commonwealth nations)  =  2,219.4 cubic inches

    Conversion Factors
    1 cubic centimeter  =  0.06 cubic inch
    1 cubic inch  =  16.4 cubic centimeters
    1 cubic yard  =  0.765 cubic meter
    1 cubic meter  =  1.3 cubic yards
    1 milliliter  =  0.034 fluid ounce
    1 fluid ounce  =  29.6 milliliters
    1 U.S. quart  =  0.946 liter
    1 liter  =  1.06 U.S. quarts
    1 U.S. gallon  =  3.8 liters
    1 imperial gallon  =  1.2 U.S. gallons  =  4.5 liters
    1 liter  =  0.9 dry quart
    1 dry quart  =  1.1 liters
    1 dekaliter  =  0.28 U.S. bushel
    1 U.S. bushel  =  0.97 imperial bushel  =  3.5 dekaliters

    Weight (Mass)
    Metric System
    1 ml milligram  =  1/1,000,000 kilogram  =  1/1,000 gram
    1 cg centigram  =  1/100,000 kilogram  =  1/100 gram
    1 dg decigram  =  1/10,000 kilogram  =  1/10 gram
    1 g  gram  =  1/1,000 kilogram
    1 dg dekagram  =  1/100 kilogram  =  10 grams
    1 hg hectogram  =  1/10 kilogram  =  100 grams
    1 kl kilogram (basic unit of weight or mass)
    1 t  metric ton  =  1,000 kilograms

    American and British Units: Avoirdupois
    1 grain  =  1/7,000 pound  =  1/437.5 ounce
    1 dram  =  1/256 pound  =  1/16 ounce
    1 ounce  =  1/16 pound
    1 pound (basic unit of weight or mass)
    1 short hundredweight  =  100 pounds
    1 long hundredweight  =  112 pounds
    1 short ton  =  2,000 pounds
    1 long ton  =  2,240 pounds

    American and British Units: Troy and Apothecaries'
    1 grain  =  1/7,000 avoirdupois pound  =  1/5,760 troy or apothecaries' pound
    1 apothecaries' scruple  =  20 grains  =  1/3 dram
    1 pennyweight  =  24 grains  =  1/20 troy ounce
    1 apothecaries' dram  =  60 grains  =  1/8 apothecaries' ounce
    1 troy or apothecaries' ounce  =  480 grains  =  1/12 troy or apothecaries' pound
    1 troy or apothecaries' pound  =  5,760 grains  =  5,760/7,000 avoirdupois pound

    Conversion Factors
    1 milligram  =  0.015 grain
    1 grain  =  64.8 milligrams
    1 gram  =  0.035 avoirdupois ounce
    1 avoirdupois ounce  =  28.35 grams
    1 troy or apothecaries' pound  =  0.82 avoirdupois pound  =  0.37 kilogram
    1 avoirdupois pound  =  1.2 troy or apothecaries' pounds  =  0.45 kilogram
    1 kilogram  =  2.205 avoirdupois pounds
    1 short ton  =  0.9 metric ton
    1 metric ton  =  1.1 short tons
*/
}
