using RGiesecke.DllExport;
using System;
using System.Runtime.InteropServices;

namespace ImporterSample
{
    // Much of this is from imc's file named ImcExtensionsInterface.h
    public static class SampleImporter
    {
        // ------------------------------------------------------------------------------------------------
        // Description: 
        //     Parameter values for <Imc_IsExtensionDLL>.
        // Remarks:
        //     These values identify a certain kind of imc extension DLL. 
        // ------------------------------------------------------------------------------------------------
        public enum IMCEXTENSION_TYPE
        {
            IMCEXTENSION_IMPORTER = 1,  // DLL implements the interface required for an import DLL.
            IMCEXTENSION_EXPORTER = 2   // DLL implements the interface required for an export DLL.   
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //     Imc_IsExtensionDLL
        // Description:
        //     Identify the library as a pluggable imc extension DLL.        
        // Parameters: 
        //     Kind: Specifies the requested kind of imc extension DLL. See IMCEXTENSION_TYPE for 
        //           a list of defined types.
        // Returns:
        //    TRUE, if the DLL supports the requested kind of extension, FALSE otherwise.
        // 
        // Remarks: 
        //    Please note that the IMCEXTENSION_TYPE enumeration may be extended in future versions.
        //    Thus return always false for unknown <Kind> parameters.
        // 
        // This function is always required.
        // {group:General}
        // ------------------------------------------------------------------------------------------------
        [DllExport("Imc_IsExtensionDLL", CallingConvention = CallingConvention.Cdecl)]
        public static bool Imc_IsExtensionDLL(uint Kind)
        {
            System.Diagnostics.Debug.WriteLine("Imc_IsExtensionDLL");
            if (Kind == (uint)IMCEXTENSION_TYPE.IMCEXTENSION_IMPORTER)
                return true;
            else
                return false;
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //     Imc_LoginExtensionDLL
        // Description:
        //     This function is called by each user who wants to use the library. 
        // Parameters: 
        //     RequestedLanguage: The requested language for error messages and user interfaces.
        //     Reserved: Always zero.
        // Returns:
        //      TRUE, if the function succeeds, FALSE otherwise.
        //      Caller can get extended error information by calling <Imc_GetLastError>.
        // 
        // Remarks: 
        //      + Typically called after the library has been loaded with LoadLibrary().
        //      + This is the right place for time-consuming initializations, LoadLibrary calls etc.
        //      + Be aware that this function could be called multiple times (if there are multiple 
        //        users in the same process), so you should provide an internal "login counter"
        //        and perform initializations only for the first login.
        //      + Each successful call of Imc_LoginExtensionDLL requires an matching call of 
        //        Imc_LogoutExtensionDLL.
        //
        // 
        // {group:General}
        // ------------------------------------------------------------------------------------------------
        [DllExport("Imc_LoginExtensionDLL", CallingConvention = CallingConvention.Cdecl)]
        public static bool Imc_LoginExtensionDLL(UInt32 RequestedLanguage, UInt64 dwReserved)
        {
            System.Diagnostics.Debug.WriteLine("Imc_LoginExtensionDLL");
            return true;
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //     Imc_LogoutExtensionDLL
        // Description:
        //     This function is called by each user before freeing the library.
        // Returns:
        //     None
        // 
        // Remarks: 
        //      + Typically called before the library is unloaded with FreeLibrary().
        //      + This is the right place for freeing any resources, FreeLibrary calls etc.
        //      + Be aware that this function could be called multiple times (if there are multiple 
        //        users in the same process), so you should provide an internal "login counter"
        //        and perform your cleanup work only when the last user quits.
        //           
        // 
        // {group:General}
        // ------------------------------------------------------------------------------------------------
        [DllExport("Imc_LogoutExtensionDLL", CallingConvention = CallingConvention.Cdecl)]
        public static void Imc_LogoutExtensionDLL()
        {
            System.Diagnostics.Debug.WriteLine("Imc_LogoutExtensionDLL");
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //      Imc_GetLastError
        // Description: 
        //      Returns the description for the last error.
        // Parameters:
        //   ErrorDescription:   
        //      [in, out] Buffer to receive the error description.
        //   ErrorDescriptionSize:
        //      [in] The length, in chars, of the buffer to which the <ErrorDescription> parameter points.
        // Returns:
        //       None.
        // Remarks:
        //     + Whenever a funtion returns a value indicating an error, the DLL is responsible for 
        //     storing a meaningful error description. The caller can retrieve this description
        //     with this function. 
        //     + Be aware that the DLL may be used under different Windows user interface languages, so we 
        //     recommend using English as the default language or supporting multiple languages. In the latter 
        //     case, check the current Windows UI language to find the best language at runtime.
        //     + The length of an error message should not exceed 256 characters.
        // 
        // {group:General}
        // ------------------------------------------------------------------------------------------------
        [DllExport("Imc_GetLastError", CallingConvention = CallingConvention.Cdecl)]
        public static void Imc_GetLastError(string pBuffer, int cbBuffSize)
        {
            System.Diagnostics.Debug.WriteLine("Imc_GetLastError");
        }

        // ------------------------------------------------------------------------------------------------
        // Function: 
        //    ImcImp_GetFormatCount
        // Description:
        //    Returns the count of all available import formats. 
        // Parameters: 
        //    None.
        // Returns:
        //    The count of supported import formats. 
        // Remarks:
        //    Used by FAMOS in conjunction with <ImcImp_GetFormatInfo> 
        //    by FAMOS to enumerate all available importers.
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_GetFormatCount", CallingConvention = CallingConvention.Cdecl)]
        public static int ImcImp_GetFormatCount()
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_GetFormatCount");
            return 1;
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //    ImcImp_GetFormatInfo
        // Description:
        //    Returns information about an import filter. 
        // 
        // Parameters:
        //      Index:       [in]       Index of the required import format. The first valid index is 0, the highest value 
        //                              is the return value of ImcImp_GetFormatCount()-1.
        //      FormatName:  [in,out]   Buffer to receive the Format identifier.
        //      FormatNameSize:[in]       The length, in chars, of the buffer to which the parameter <FormatName> points.
        //      DefExtension:[in,out]   Buffer to receive the default file extension(s) for the files.
        //                              Multiple extensions must be separated by a semicolon, e.g. "txt;asc"  
        //      DefExtensionSize:[in]     The length, in chars, of the buffer to which the <DefExtension> parameter points.
        //      FDescription:   [in,out] Buffer to receive the description of the import format.
        //      FDescriptionSize: [in]     The length, in chars, of the buffer to which the <Description> parameter points.
        //      IsConfigurable:[in,out] Set to TRUE, if this filter is configurable, i.e. the function 
        //                              <ImcImp_ConfigurationDialog> is implemented. 
        //                              FALSE otherwise
        // Returns:
        //      TRUE, if the function succeeds, FALSE otherwise.
        //      Caller can get extended error information by calling <Imc_GetLastError>.
        //      calling 
        //
        // Remarks:
        //    Returns information about an import format. 
        //    Used by FAMOS in conjunction with <ImcImp_GetFormatCount> 
        //    by FAMOS to enumerate all available importers.
        //
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_GetFormatInfo", CallingConvention = CallingConvention.Cdecl)]
        public static bool ImcImp_GetFormatInfo(int index,
            IntPtr FormatName, int FormatNameSize,
            IntPtr DefExtensions, int DefExtensionsSize,
            IntPtr FormatComment, int FormatCommentSize,
            IntPtr isConfigurablePointer)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_GetFormatInfo");
            /*
            if (index == 1)
            {
                FormatName = NullTerminateAString("Testing...", FormatNameSize).ToCharArray();
                DefExtensions = NullTerminateAString("txt", DefExtensionsSize).ToCharArray();
                FormatComment = NullTerminateAString("Nuttin", FormatCommentSize).ToCharArray();
                isConfigurablePointer = default(IntPtr);
            }
            else
             * */
            return false;
        }

        private static string NullTerminateAString(string inputString, int numberOfCharacters)
        {
            string result = inputString.Substring(0, Math.Min(numberOfCharacters - 1, inputString.Length)).PadRight(numberOfCharacters, (char)0);
            return result;
        }

        // ------------------------------------------------------------------------------------------------
        // Function: 
        //    ImcImp_ConfigurationDialog
        // Description:
        //    Shows an modal dialog with options for the import process.
        // Parameters:
        //    FormatName:
        //       [in] Name of the requested import format. Matches the respective name returned by 
        //       <ImcImp_GetFormatInfo>.
        //    hwndOwner:
        //        [in] Owner window for the modal dialog.
        //    CurrentFileSelection:
        //        [in] When called by the "Load file" dialog in FAMOS, this parameter contains the 
        //             current file selection from the dialog. See remarks section for details.
        // Returns:
        //      TRUE, if the function succeeds, FALSE otherwise.
        //      Caller can get extended error information by calling <Imc_GetLastError>.
        // Remarks:
        //      FAMOS calls this function when the user hits the "Option" button in the "Load file" 
        //      dialog.
        //      The DLL is responsible for the persistence of the settings the user made. These 
        //      settings apply to all following calls of <ImcImp_OpenFile>.
        //
        //      The parameter <CurrentFileSelection> is intended for filters which require 
        //      options on a "per file basis" rather than global format options. 
        //      The string is built from the current directory, followed by the current contents of 
        //      the dialog's filename edit box. Always check for NULL, empty strings or inconsistent filenames!
        //
        //      Typical format of <CurrentFileSelection>: 
        // @pre
        // Single selection=>  c:\files\file1
        // Multi selection=>   c:\files\"file1" "file2" "file3"
        // @endpre
        //
        // Note:
        //      This function must be implemented, when this format has been marked as "configurable".
        //      (Last parameter of <ImcImp_GetFormatInfo> is set to TRUE.)
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_ConfigurationDialog", CallingConvention = CallingConvention.Cdecl)]
        public static bool ImcImp_ConfigurationDialog(string pszFormatName, IntPtr hwndOwner, string pszCurrentFileSelection)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_ConfigurationDialog");
            return false; // not implemented
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //      ImcImp_OpenFile
        // Description:
        //     Opens a file for data import.
        // Parameters:
        //       FileName:   
        //           [in] Complete path name for the file from which to import. 
        //       FormatName:
        //           [in] Name of the requested import format. Matches the respective name delivered by 
        //           <ImcImp_GetFormatInfo>.
        //       MoreParameters: 
        //           [in] Additional parameters given by the user (2nd parameter of the FileOpenFAS() function
        //                in FAMOS).
        //       Options:
        //           [in] Additional options given by the user (3rd parameter of the FileOpenFAS() function 
        //                in FAMOS). For a list of predefined values, which may be combined, see the IMCIMPORT_FLAGS 
        //                enumeration.
        //       hWndOwner:
        //           [in] Parent window for dialogs.
        //       CallContext:
        //           [in] Information about the caller of the function. 
        //                For a list of possible values, see the IMCEXTENSION_CALLCONTEXT enumeration.
        // Returns:
        //      Handle identifying the import if the function succeeds. Always use the ImcImp_CloseFile function 
        //      to close the handle and finish the import. 
        //      Returns 0 in case of an error. Caller can get extended error information by calling <Imc_GetLastError>.
        // 
        // Remarks:
        //      The returned handle is a "black box" read-only object for the caller and only 
        //      used by subsequent function calls accessing this file (so it must be unique). 
        //      The real meaning of this handle is arbitrary. It could be an index to an internal
        //      table, a handle to memory block, a "real" file handle...
        // 
        //  
        // 
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_OpenFile", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr ImcImp_OpenFile(string pszFileName, string pszFormatName, string pszConfigFile, uint dwOption, IntPtr hwndOwner, uint CallContext)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_OpenFile");
            return default(IntPtr);
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //      ImcImp_GetDataObjectCount
        // Description:
        //     Retrieves the count of data objects in an opened file.
        // Parameters:
        //       Handle:   
        //           [in] Unique identifier for the file, returned by an previous call of
        //                <ImcImp_OpenFile>
        //       
        // Returns:
        //      Count of data objects, if the function succeeds. 0 otherwise.
        //      Caller can get extended error information by calling <Imc_GetLastError>.
        // 
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_GetDataObjectCount", CallingConvention = CallingConvention.Cdecl)]
        public static int ImcImp_GetDataObjectCount(IntPtr Handle)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_GetDataObjectCount");
            return 0; //?????
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //    ImcImp_GetDataObject
        // Description:
        //     Retrieves a data object from an opened file.
        // Parameters:
        //       Handle:   
        //           [in] Unique identifier for the file, returned by an previous call of
        //                <ImcImp_OpenFile>
        //        Index: 
        //            [in] Index of the required data object. The first valid index is 0, the highest value 
        //                 is the return value of ImcExp_GetDataObjectCount()-1.
        /// 
        // Returns:
        //       Handle of the data object if the function succeeds. NULL otherwise,
        //       Caller can get extended error information by calling <Imc_GetLastError>.
        //
        // Remarks:
        //       The caller takes the ownership of the returned object.
        // 
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_GetDataObject", CallingConvention = CallingConvention.Cdecl)]
        public static IntPtr ImcImp_GetDataObject(IntPtr Handle, Int64 index)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_GetDataObject");
            return default(IntPtr); //?????
        }

        // ------------------------------------------------------------------------------------------------
        // Function:
        //      ImcImp_CloseFile   
        // Description: 
        //      Closes an open import file.
        // Parameters:
        //    Handle:   
        //       [in] Unique identifier for the file, returned by an previous call of
        //            <ImcImp_OpenFile>
        // Returns: 
        //      TRUE, if the function succeeds, FALSE otherwise.
        //      Caller can get extended error information by calling <Imc_GetLastError>.
        // Remarks:
        //   This function must be called after each successful ImcImp_OpenFile. 
        //   The importer must free all previous reserved resources here.
        // {group:Import (Mandatory)}
        // ------------------------------------------------------------------------------------------------
        [DllExport("ImcImp_CloseFile", CallingConvention = CallingConvention.Cdecl)]
        public static bool ImcImp_CloseFile(IntPtr Handle)
        {
            System.Diagnostics.Debug.WriteLine("ImcImp_CloseFile");
            return false; //?????
        }
    }
}
