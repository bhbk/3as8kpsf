#ifndef __PRINT__
#define __PRINT__
/*******************************************************/

#define SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE 4096

static LPTSTR SERVICESNAPSHOT_TRAMPOLINE_PATH = TEXT("\\\\.\\Pipe\\Ludus");

/*******************************************************/

INT _EvalException(INT n_except);
VOID _PrintEnter(const CHAR *psz, ...);
VOID _PrintExit(const CHAR *psz, ...);
VOID _Print(const CHAR *psz, ...);
VOID _Pipe(PCSTR pszMsg);
VOID _PrintSafe(PCSTR pszMsg, va_list args);
PCHAR do_base(PCHAR pszOut, UINT64 nValue, UINT nBase, PCSTR pszDigits);
PCHAR do_str(PCHAR pszOut, PCSTR pszIn);
PCHAR do_wstr(PCHAR pszOut, PCWSTR pszIn);

/*******************************************************/
#endif