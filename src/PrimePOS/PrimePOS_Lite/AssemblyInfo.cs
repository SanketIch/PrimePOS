using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PrimePOS")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Micro Merchant Systems, Inc.")]
[assembly: AssemblyProduct("PrimePOS")]
[assembly: AssemblyCopyright("Copyright ©  2023, MicroMerchant Systems")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers
// by using the '*' as shown below:

//2/22/2011 - 1.2.4.6   - Enhanced Change Due window to display more information
//                      - Changed behaviour of zero amount transaction to show Change Due Screen
//                      - Fixed bug in House Charge when house charge was obtained from patient and the account# did not exist in Account table
//
//5/6/2011  - 1.3.0.0   - PCI compliant Security
//                      - Added option to set selling price based on various criteria related to cost and price
//                      - Enhanced Item add option in transaction screen to allow for full, simple or necessary fields in item file
//                      - Added option to not print the station close# as well as eod# on st close and eod reports
//                      - Added Customer Loyalty Program Option
//                      - Misc. bug fixes
//
//1.3.0.1  - 5/16/2011  - Added option to retrieve CC info from charge account in primerx on the payment screen by pressing F10
//                      - Enhanced Customer Loyalty option to be more user friendly.
//
//1.3.0.2  - 5/26/2011  - Added option to retreive CC info from Patient Pay pref in PrimeRx
//                      - Added option in User File to reset user password to default (POS)
//                      - Added option in User File to Unlock a locked user account
//                      - Fixed Max Attempts related Login bugs
//                      - Partial returns now can be done through calling original transaction using transaction#
//                      - If user edits Item price, description or tax code from Transaction screen for an item in the transaction, the system now allows user to reflect those changes in the transaction
//                      - Proper handling of discount when discount for an item is more than the price

//
// In order to sign your assembly you must specify a key to use. Refer to the
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing.
//
// Notes:
//   (*) If no key is specified, the assembly is not signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. KeyFile refers to a file which contains
//       a key.
//   (*) If the KeyFile and the KeyName values are both specified, the
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP, that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key
//           in the KeyFile is installed into the CSP and used.
//   (*) In order to create a KeyFile, you can use the sn.exe (Strong Name) utility.
//       When specifying the KeyFile, the location of the KeyFile should be
//       relative to the project output directory which is
//       %Project Directory%\obj\<configuration>. For example, if your KeyFile is
//       located in the project directory, you would specify the AssemblyKeyFile
//       attribute as [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
[assembly: AssemblyVersion("5.0.9.2")]
[assembly: AssemblyFileVersion("5.0.9.2")]