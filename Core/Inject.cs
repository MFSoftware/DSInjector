
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DSInjector
{
  internal class Inject
  {
    [DllImport("kernel32")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, UIntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);

    [DllImport("kernel32.dll")]
    public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, int dwProcessId);

    [DllImport("kernel32.dll")]
    public static extern int CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint dwFreeType);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
    public static extern UIntPtr GetProcAddress(IntPtr hModule, string procName);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    [DllImport("kernel32.dll")]
    private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, string lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32", SetLastError = true)]
    internal static extern int WaitForSingleObject(IntPtr handle, int milliseconds);

    public static int GetProcessId(string proc)
    {
      return Process.GetProcessesByName(proc)[0].Id;
    }

    public static unsafe void InjectDLL(IntPtr hProcess, string strDLLName)
    {
      int num1 = strDLLName.Length + 1;
      IntPtr num2 = Inject.VirtualAllocEx(hProcess, (IntPtr) ((void*) null), (uint) num1, 4096U, 64U);
      IntPtr num3;
      Inject.WriteProcessMemory(hProcess, num2, strDLLName, (UIntPtr) ((ulong) num1), out num3);
      UIntPtr procAddress = Inject.GetProcAddress(Inject.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
      IntPtr remoteThread = Inject.CreateRemoteThread(hProcess, (IntPtr) ((void*) null), 0U, procAddress, num2, 0U, out num3);
      int num4 = Inject.WaitForSingleObject(remoteThread, 10000);
      int num5;
      switch (num4)
      {
        case 128:
        case 258:
          num5 = 0;
          break;
        default:
          num5 = num4 != -1 ? 1 : 0;
          break;
      }
      if (num5 == 0)
      {
        int num6 = (int) MessageBox.Show(" hThread [ 2 ] Error! \n ");
        Inject.CloseHandle(remoteThread);
      }
      else
      {
        Thread.Sleep(1000);
        Inject.VirtualFreeEx(hProcess, num2, (UIntPtr) 0U, 32768U);
        Inject.CloseHandle(remoteThread);
      }
    }
  }
}
