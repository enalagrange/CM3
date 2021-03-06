﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.AppearanceModule.Files
{
	using ConceptMatrix;

	#pragma warning disable IDE1006, SA1300
	public class LegacyAppearanceFile : LegacyEquipmentSetFile
	{
		public static readonly FileType AllFileType = new FileType("cma", "CM2 All Appearance File", typeof(LegacyAppearanceFile));

		public string CharacterBytes { get; set; }
		public Details characterDetails { get; set; }

		public new FileBase Upgrade()
		{
			AppearanceFile allFile = base.Upgrade();

			byte[] data = this.StringtoBytes(this.CharacterBytes);

			// From CM2: MainWindow.xaml.cs line 708
			allFile.Race = (Appearance.Races)data[0];
			allFile.Gender = (Appearance.Genders)data[1];
			allFile.Age = (Appearance.Ages)data[2];
			allFile.Height = data[3];
			allFile.Tribe = (Appearance.Tribes)data[4];
			allFile.Head = data[5];
			allFile.Hair = data[6];
			allFile.EnableHighlights = data[7] != 0;
			allFile.Skintone = data[8];
			allFile.REyeColor = data[9];
			allFile.HairTone = data[10];
			allFile.Highlights = data[11];
			allFile.FacialFeatures = (Appearance.FacialFeature)data[12];
			allFile.LimbalEyes = data[13];
			allFile.Eyebrows = data[14];
			allFile.LEyeColor = data[15];
			allFile.Eyes = data[16];
			allFile.Nose = data[17];
			allFile.Jaw = data[18];
			allFile.Mouth = data[19];
			allFile.LipsToneFurPattern = data[20];
			allFile.EarMuscleTailSize = data[21];
			allFile.TailEarsType = data[22];
			allFile.Bust = data[23];
			allFile.FacePaint = data[24];
			allFile.FacePaintColor = data[25];

			/*
			allFile.SkinTint = new Color((float)this.characterDetails.SkinRedPigment.value, (float)this.characterDetails.SkinGreenPigment.value, (float)this.characterDetails.SkinBluePigment.value);
			allFile.SkinGlow = new Color((float)this.characterDetails.SkinRedGloss.value, (float)this.characterDetails.SkinGreenGloss.value, (float)this.characterDetails.SkinBlueGloss.value);
			allFile.LeftEyeColor = new Color((float)this.characterDetails.LeftEyeRed.value, (float)this.characterDetails.LeftEyeGreen.value, (float)this.characterDetails.LeftEyeBlue.value);
			allFile.RightEyeColor = new Color((float)this.characterDetails.RightEyeRed.value, (float)this.characterDetails.RightEyeGreen.value, (float)this.characterDetails.RightEyeBlue.value);
			allFile.LimbalRingColor = new Color((float)this.characterDetails.LimbalR.value, (float)this.characterDetails.LimbalG.value, (float)this.characterDetails.LimbalB.value);
			allFile.HairTint = new Color((float)this.characterDetails.HairRedPigment.value, (float)this.characterDetails.HairGreenPigment.value, (float)this.characterDetails.HairBluePigment.value);
			allFile.HairGlow = new Color((float)this.characterDetails.HairGlowRed.value, (float)this.characterDetails.HairGlowGreen.value, (float)this.characterDetails.HairGlowBlue.value);
			allFile.HighlightTint = new Color((float)this.characterDetails.HighlightRedPigment.value, (float)this.characterDetails.HighlightGreenPigment.value, (float)this.characterDetails.HighlightBluePigment.value);
			allFile.LipTint = new Color4((float)this.characterDetails.LipsR.value, (float)this.characterDetails.LipsG.value, (float)this.characterDetails.LipsB.value, (float)this.characterDetails.LipsBrightness.value);
			*/

			return allFile;
		}

		public override FileType GetFileType()
		{
			return AllFileType;
		}

		public class Details
		{
			public Entry TailSize { get; set; }
			public Entry Height { get; set; }
			public Entry BustX { get; set; }
			public Entry BustY { get; set; }
			public Entry BustZ { get; set; }
			public Entry Voices { get; set; }
			public Entry MuscleTone { get; set; }
			public Entry WeaponX { get; set; }
			public Entry WeaponY { get; set; }
			public Entry WeaponZ { get; set; }
			public Entry OffHandX { get; set; }
			public Entry OffHandY { get; set; }
			public Entry OffHandZ { get; set; }
			public Entry OffHandRed { get; set; }
			public Entry OffHandGreen { get; set; }
			public Entry OffHandBlue { get; set; }
			public Entry WeaponRed { get; set; }
			public Entry WeaponGreen { get; set; }
			public Entry WeaponBlue { get; set; }
			public Entry SkinRedPigment { get; set; }
			public Entry SkinGreenPigment { get; set; }
			public Entry SkinBluePigment { get; set; }
			public Entry SkinRedGloss { get; set; }
			public Entry SkinGreenGloss { get; set; }
			public Entry SkinBlueGloss { get; set; }
			public Entry HairRedPigment { get; set; }
			public Entry HairGreenPigment { get; set; }
			public Entry HairBluePigment { get; set; }
			public Entry HairGlowRed { get; set; }
			public Entry HairGlowGreen { get; set; }
			public Entry HairGlowBlue { get; set; }
			public Entry HighlightRedPigment { get; set; }
			public Entry HighlightGreenPigment { get; set; }
			public Entry HighlightBluePigment { get; set; }
			public Entry LeftEyeRed { get; set; }
			public Entry LeftEyeGreen { get; set; }
			public Entry LeftEyeBlue { get; set; }
			public Entry RightEyeRed { get; set; }
			public Entry RightEyeGreen { get; set; }
			public Entry RightEyeBlue { get; set; }
			public Entry LipsBrightness { get; set; }
			public Entry LipsR { get; set; }
			public Entry LipsG { get; set; }
			public Entry LipsB { get; set; }
			public Entry LimbalR { get; set; }
			public Entry LimbalG { get; set; }
			public Entry LimbalB { get; set; }
		}

		public class Entry
		{
			public double value { get; set; }
		}
	}
}
