﻿// Christopher Braun 2016

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using FMA.Contracts;
using FMA.View.Helpers;
using FMA.View.Models;

namespace FMA.View.Common
{
    public class FlyerMakerViewModel : FlyerMakerViewModelBase
    {
        public event Action<CustomMaterial> FlyerCreated = m => { };

        public FlyerMakerViewModel(List<Material> materials, int selectedMateriaId, Func<string, FontInfo> getFont,
            IFontService fontService, IWindowService windowService)
            : base(materials, getFont, fontService, windowService)
        {
            SetMaterials(materials, selectedMateriaId, fontService);
            LayoutMode = false;
        }

        private void SetMaterials(IEnumerable<Material> materials, int selectedMateriaId, IFontService fontService)
        {
            Materials = materials.Select(m => m.ToMaterialModel(fontService)).ToList();
            if (selectedMateriaId != -1)
            {
                SelectedMaterial = Materials.Single(m => m.Id.Equals(selectedMateriaId));
            }
        }


        private bool layoutButtonsVisible;

        public bool LayoutButtonsVisible
        {
            get { return layoutButtonsVisible; }
            set
            {
                layoutButtonsVisible = value;
                OnPropertyChanged();
            }
        }

        public void EnableLayoutButtons()
        {
            if (Keyboard.Modifiers == (ModifierKeys.Shift | ModifierKeys.Control))
            {
                LayoutButtonsVisible = !LayoutButtonsVisible;
            }
        }

        public void Create()
        {
            FlyerCreated(SelectedMaterial.ToCustomMaterial());
        }
    }
}