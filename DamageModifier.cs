using Modding;
using GlobalEnums;
using Satchel.BetterMenus;
using System;

// https://www.bilibili.com/video/BV1jWkKBJEkD
namespace DamageModifier
{
    public class GlobalSettings
    {
        public int CloseDamage = 0;
    }

    public class DamageModifier : Mod, IGlobalSettings<GlobalSettings>, ICustomMenuMod, ITogglableMod
    {
        private Menu MenuRef;

        private static DamageModifier? _instance;
        public bool ToggleButtonInsideMenu => true;

        public static GlobalSettings GS { get; set; } = new GlobalSettings();

        new public string GetName() => "DamageModifier";

        public override string GetVersion() => "1.0.0";

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modToggleDelegates)
        {
            if (MenuRef == null)
            {
                MenuRef = new Menu("DamageModifier", new Element[]
                {
                    Blueprints.CreateToggle(
                        modToggleDelegates.Value,
                        "Mod Enabled",
                        ""
                        ),
                    new HorizontalOption(
                        "CloseDamage?",
                        "",
                        new string[] {"Yes","No"},
                        (setting) =>
                        {
                            GS.CloseDamage = setting;
                        },
                        () => GS.CloseDamage
                        )
                }
                );
            }
            return MenuRef.GetMenuScreen(modListMenu);
        }

        internal static DamageModifier Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"{nameof(DamageModifier)} was never initialized");
                }
                return _instance;
            }
        }

        public DamageModifier() : base()
        {
            _instance = this;
        }

        // if you need preloads, you will need to implement GetPreloadNames and use the other signature of Initialize.
        public override void Initialize()
        {
            Log("Initializing");

            ModHooks.TakeDamageHook += this.OnTakeDamageProxy;

            Log("Initialized");
        }

        private int OnTakeDamageProxy(ref int hazardType, int damage)
        {
            return 0;
        }

        // Code that should be run when the mod is disabled.
        public void Unload()
        {
            // Unhook the methods previously registered so no exceptions will happen.
            ModHooks.TakeDamageHook -= this.OnTakeDamageProxy;
            // "Destroy" the loaded instance of the mod.
            _instance = null;
        }

        public void OnLoadGlobal(GlobalSettings s) => GS = s;

        public GlobalSettings OnSaveGlobal() => GS;

    }
}