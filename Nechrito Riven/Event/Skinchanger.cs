#region

using System;
using NechritoRiven.Menus;

#endregion

namespace NechritoRiven.Event
{
    internal class Skinchanger : Core.Core
    {
        public static void Update(EventArgs args)
        {
            Player.SetSkin(Player.CharData.BaseSkinName, MenuConfig.UseSkin ? MenuConfig.SkinList.SelectedIndex : Player.BaseSkinId);
        }
    }
}
