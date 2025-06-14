using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calypso
{
    internal class LayoutManager : LayoutPresets
    {
        public static LayoutManager i { get; private set; }

        private SplitContainer tagTreeSplitContainer;
        private SplitContainer masterSplitContainer;
        private SplitContainer imageInfoHorizontalSplitContainer;

        int metadataHSplitter_Distance;
        int leftPanelVSplitter_Distance;
        int rightPanelVSplitter_Distance;

        //bool metadata_IsOpen;
        //bool leftPanel_IsOpen;
        //bool rightPanel_IsOpen;


        MainWindow? mainW;
        public LayoutManager(MainWindow mainW)
        {
            if (i != null)
                throw new InvalidOperationException($"Cannot have two instances of {this}");

            i = this;
            this.mainW = mainW;

            i.tagTreeSplitContainer = mainW.tagTreeGallerySplitContainer;
            i.imageInfoHorizontalSplitContainer = mainW.imageInfoHorizontalSplitContainer;
            i.masterSplitContainer = mainW.masterSplitContainer;
        }

        public void LoadLayout(LayoutData ld)
        {
            SetPanel(i.masterSplitContainer, 2, ld.RightPanel_IsOpen);
            SetPanel(i.tagTreeSplitContainer, 1, ld.LeftPanel_IsOpen);
            SetPanel(i.imageInfoHorizontalSplitContainer, 2, ld.Metadata_IsOpen);

            tagTreeSplitContainer.SplitterDistance = (int)Math.Round(tagTreeSplitContainer.Width * ld.LeftPanelHSplitter_Ratio);
            masterSplitContainer.SplitterDistance = (int)Math.Round(masterSplitContainer.Width * ld.RightPanelHSplitter_Ratio);
            imageInfoHorizontalSplitContainer.SplitterDistance = (int)Math.Round(imageInfoHorizontalSplitContainer.Height * ld.MetadataVSplitter_Ratio);
        }

        public void SaveLayout()
        {

        }

        // managing methods -----------------------------------------------------------------------

        public void SetPanel(SplitContainer splitContainer, int panelNumber, bool value)
        {
            //// if re-opening the splitter, set the distance 
            //int splitterDistance = 0;
            //if (value)
            //{
            //    if (splitContainer is metadataHSplitter_Distance)
            //        splitterDistance = metadataHSplitter_Distance
            //}
            //    splitContainer.SplitterDistance = 

            if (panelNumber == 1)
                splitContainer.Panel1Collapsed = !value;
            else
                splitContainer.Panel2Collapsed = !value;
        }

        public void TogglePanel(SplitContainer splitContainer, int panelNumber) 
        {
            SetPanel(splitContainer, panelNumber, panelNumber == 1? splitContainer.Panel1Collapsed : splitContainer.Panel2Collapsed);
        }


    }
}
