﻿using System.Collections.Generic;
using System.Linq;

namespace FMA.Contracts
{
    public class CustomMaterial
    {
        public CustomMaterial(int id, string title, string description, IEnumerable<MaterialField> materialFields,
            byte[] flyerFrontSide, byte[] flyerBackside)
        {
            Id = id;
            Title = title;
            Description = description;

            this.MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;
        }

        public CustomMaterial(int id, string title, string description, IEnumerable<MaterialField> materialFields, byte[] flyerFrontSide, byte[] flyerBackside, byte[] logo, double logoLeftMargin, double logoTopMargin, double logoWidth, double logoHeight)
        {
            Id = id;
            Title = title;
            Description = description;

            this.MaterialFields = materialFields.ToList();

            FlyerFrontSide = flyerFrontSide;
            FlyerBackside = flyerBackside;

            Logo = logo;
            LogoLeftMargin = logoLeftMargin;
            LogoTopMargin = logoTopMargin;
            LogoWidth = logoWidth;
            LogoHeight = logoHeight;
        }

        public int Id { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public IEnumerable<MaterialField> MaterialFields { get; private set; }


        public byte[] FlyerFrontSide { get; private set; }

        public byte[] FlyerBackside    { get; private set; }


        public byte[] Logo { get; set; }

        public double LogoLeftMargin { get; set; }
        public double LogoTopMargin { get; set; }
        public double LogoWidth { get; set; }
        public double LogoHeight { get; set; }
    }
}
