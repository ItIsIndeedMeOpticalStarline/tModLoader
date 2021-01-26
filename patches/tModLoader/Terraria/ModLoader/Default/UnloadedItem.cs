using System.Collections.Generic;
using System.IO;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default
{
	[LegacyName("MysteryItem")]
	public class UnloadedItem : ModLoaderModItem
	{
		private string modName;
		private string itemName;
		private TagCompound data;

		public override void SetStaticDefaults() {
			DisplayName.SetDefault("{$tModLoader.UnloadedItemItemName}");
			Tooltip.SetDefault("\n");
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.rare = 1;
		}

		internal void Setup(TagCompound tag) {
			modName = tag.GetString("mod");
			itemName = tag.GetString("name");
			data = tag;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips) {
			for (int k = 0; k < tooltips.Count; k++) {
				if (tooltips[k].Name == "Tooltip0") {
					tooltips[k].text = Language.GetTextValue("tModLoader.UnloadedItemModTooltip", modName);
				}
				else if (tooltips[k].Name == "Tooltip1") {
					tooltips[k].text = Language.GetTextValue("tModLoader.UnloadedItemItemNameTooltip", itemName);
				}
			}
		}

		public override TagCompound Save() {
			return data;
		}

		public override void Load(TagCompound tag) {
			Setup(tag);
			if (ModContent.TryFind(modName, itemName, out ModItem modItem)) {
				Item.SetDefaults(modItem.Type);
				Item.ModItem.Load(tag.GetCompound("data"));
				ItemIO.LoadGlobals(Item, tag.GetList<TagCompound>("globalData"));
			}
		}

		public override void NetSend(BinaryWriter writer) {
			TagIO.Write(data ?? new TagCompound(), writer);
		}

		public override void NetReceive(BinaryReader reader) {
			Setup(TagIO.Read(reader));
		}

		public override ModItem Clone(Item item) {
			var clone = (UnloadedItem)base.Clone(item);
			clone.data = (TagCompound)data?.Clone();
			return clone;
		}
	}
}
