// Decompiled with JetBrains decompiler
// Type: DSInjector.Properties.Settings
// Assembly: DSInjector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C99F6501-CCFB-466C-8F7A-8A3E0AD04997
// Assembly location: C:\Users\Nikita\Desktop\DS\DSInjector.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace DSInjector.Properties
{
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  [CompilerGenerated]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings defaultInstance = Settings.defaultInstance;
        return defaultInstance;
      }
    }
  }
}
