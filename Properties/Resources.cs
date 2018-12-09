// Decompiled with JetBrains decompiler
// Type: DSInjector.Properties.Resources
// Assembly: DSInjector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: C99F6501-CCFB-466C-8F7A-8A3E0AD04997
// Assembly location: C:\Users\Nikita\Desktop\DS\DSInjector.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace DSInjector.Properties
{
  [DebuggerNonUserCode]
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (DSInjector.Properties.Resources.resourceMan == null)
          DSInjector.Properties.Resources.resourceMan = new ResourceManager("DSInjector.Properties.Resources", typeof (DSInjector.Properties.Resources).Assembly);
        return DSInjector.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return DSInjector.Properties.Resources.resourceCulture;
      }
      set
      {
        DSInjector.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
