using externallol.Menu;
using System;
using System.Collections.Generic;
using System.Text;

namespace externallol.Managers
{
    public class _Managers // supporting class for loading all managers at once
    {
        public static void _Load()
        {
            CoroutineManager.Instance.StartCoroutine(AdminManager.WaitForAdminPanel());
            AdminManager.LoadWatermark();
            FolderManager.Initialize();
            SoundboardManager.LoadAllSounds(); // probably takes a while
            Boards.SetLaunchBoards();
        }
    }
}
