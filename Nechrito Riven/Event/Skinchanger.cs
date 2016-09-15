namespace NechritoRiven.Event
{
    #region

    using System;

    using Core;
    using NechritoRiven.Menus;

    #endregion

    internal class Skinchanger : Core
    {
        #region Public Methods and Operators

        public static void Update(EventArgs args)
        {
            Player.SetSkin(
                Player.CharData.BaseSkinName,
                MenuConfig.UseSkin ? MenuConfig.SkinList.SelectedIndex : Player.BaseSkinId);
        }

        #endregion
    }
}