#include <windows.h>
#include <conio.h>
#include <tchar.h>
#include <stdio.h>
#include <strsafe.h>
#include <process.h>
#include "Print.h"
#include "stdafx.h"

/************************ Logging system. ************************/
VOID _PrintEnter(const CHAR *psz, ...)
{
    DWORD dwErr = GetLastError();
    LONG nThread = 0;

	if (s_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(s_nTlsThread);
    }

    if (psz) {
        CHAR szBuf[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE];
        PCHAR pszBuf = szBuf;
        LONG nLen = 0;
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0) {
            /* Copy characters. */
        }
        _PrintSafe(szBuf, args);

        va_end(args);
    }
    SetLastError(dwErr);
}
VOID _PrintExit(const CHAR *psz, ...)
{
    DWORD dwErr = GetLastError();
    LONG nThread = 0;

    if (s_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(s_nTlsThread);
    }

    if (psz) {
        CHAR szBuf[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE];
        PCHAR pszBuf = szBuf;
        LONG nLen = 0;
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0) {
            /* Copy characters. */
        }
        _PrintSafe(szBuf, args);

        va_end(args);
    }
    SetLastError(dwErr);
}
VOID _Print(const CHAR *psz, ...)
{
    DWORD dwErr = GetLastError();
    LONG nThread = 0;

    if (s_nTlsThread >= 0) {
        nThread = (LONG)(LONG_PTR)TlsGetValue(s_nTlsThread);
    }

    if (psz) {
        CHAR szBuf[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE];
        PCHAR pszBuf = szBuf;
        LONG nLen = 0;
        while (nLen-- > 0) {
            *pszBuf++ = ' ';
        }

        va_list  args;
        va_start(args, psz);

        while ((*pszBuf++ = *psz++) != 0) {
            /* Copy characters. */
        }
        _PrintSafe(szBuf, args);

        va_end(args);
    }
    SetLastError(dwErr);
}
VOID _Pipe(PCSTR pszMsg)
{
	HANDLE hPipe; 
	CHAR chBuf[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE]; 
	BOOL fSuccess; 
	DWORD cbRead, cbWritten, dwMode; 

	// Try to open a named pipe; wait for it, if necessary.
	for(;;)
	{
		hPipe = CreateFile( 
			SERVICESNAPSHOT_TRAMPOLINE_PATH,		// pipe name.
			GENERIC_READ | GENERIC_WRITE,			// read and write access.
			0,										// no sharing.
			NULL,									// default security attributes.
			OPEN_EXISTING,							// opens existing pipe.
			0,										// default attributes.
			NULL);									// no template file.

		// Break if the pipe handle is valid.
		if (hPipe != INVALID_HANDLE_VALUE) 
		{
			break; 
		}

		// Exit if an error other than ERROR_PIPE_BUSY occurs.
		if (GetLastError() != ERROR_PIPE_BUSY) 
		{
			return;
		}

		// All pipe instances are busy, so wait for 20 seconds.
		if (!WaitNamedPipe(SERVICESNAPSHOT_TRAMPOLINE_PATH, NMPWAIT_USE_DEFAULT_WAIT)) 
		{ 
			return;
		} 
	}

	// The pipe connected; change to message-read mode.
	dwMode = PIPE_READMODE_MESSAGE; 

	fSuccess = SetNamedPipeHandleState( 
		hPipe,										// pipe handle.
		&dwMode,									// new pipe mode.
		NULL,										// don't set maximum bytes.
		NULL);										// don't set maximum time.

	if (!fSuccess) 
	{
		return;
	}

	// Prepend 'SET' command on string.
	CHAR tmpBuf[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE]; 
	PCHAR pTmpBuf = tmpBuf;
	tmpBuf[0] = '\0';

	while (*pszMsg) 
	{
		*pTmpBuf++ = *pszMsg++;
    }

	*pTmpBuf = '\0';

	pTmpBuf = tmpBuf;

	// Make sure we don't send any messages larger than the buffer size.
	if(strlen(pTmpBuf) * sizeof(CHAR) < SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE - 1) 
	{
		// Send a message to the pipe server.
		fSuccess = WriteFile( 
			hPipe,									// pipe handle.
			pTmpBuf,								// message.
			(strlen(pTmpBuf) + 1) * sizeof(CHAR),	// message length.
			&cbWritten,								// bytes written.
			NULL);									// not overlapped.

		if (!fSuccess) 
		{
			return;
		}

		do 
		{ 
			// Read from the pipe.
			fSuccess = ReadFile( 
				hPipe,													// pipe handle.
				chBuf,													// buffer to receive reply.
				SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE * sizeof(CHAR),	// size of buffer.
				&cbRead,												// number of bytes read.
				NULL);													// not overlapped.

			if (!fSuccess && GetLastError() != ERROR_MORE_DATA) 
			{
				break; 
			}
		// repeat loop if ERROR_MORE_DATA
		} while (!fSuccess);
	}
	CloseHandle(hPipe); 		
}
VOID _PrintSafe(PCSTR pszMsg, va_list args)
{
	CHAR pszBuffer[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE];
	PCHAR pszOut = pszBuffer;
    pszBuffer[0] = '\0';

    __try {
        while (*pszMsg) {
            if (*pszMsg == '%') {
                CHAR szTemp[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE];
                CHAR szHead[4] = "";
                INT nLen;
                INT nWidth = 0;
                INT nPrecision = 0;
                BOOL fLeft = FALSE;
                BOOL fPositive = FALSE;
                BOOL fPound = FALSE;
                BOOL fBlank = FALSE;
                BOOL fZero = FALSE;
                BOOL fDigit = FALSE;
                BOOL fSmall = FALSE;
                BOOL fLarge = FALSE;
                BOOL f64Bit = FALSE;
                BOOL fString = FALSE;
                PCSTR pszArg = pszMsg;

                pszMsg++;

                for (; (*pszMsg == '-' ||
                        *pszMsg == '+' ||
                        *pszMsg == '#' ||
                        *pszMsg == ' ' ||
                        *pszMsg == '0'); pszMsg++) {
                    switch (*pszMsg) {
                      case '-': fLeft = TRUE; break;
                      case '+': fPositive = TRUE; break;
                      case '#': fPound = TRUE; break;
                      case ' ': fBlank = TRUE; break;
                      case '0': fZero = TRUE; break;
                    }
                }

                if (*pszMsg == '*') {
                    nWidth = va_arg(args, INT);
                    pszMsg++;
                }
                else 
				{
                    while (*pszMsg >= '0' && *pszMsg <= '9') 
					{
                        nWidth = nWidth * 10 + (*pszMsg++ - '0');
                    }
                }
                if (*pszMsg == '.') 
				{
                    pszMsg++;
                    fDigit = TRUE;
                    if (*pszMsg == '*') 
					{
                        nPrecision = va_arg(args, INT);
                        pszMsg++;
                    }
                    else 
					{
                        while (*pszMsg >= '0' && *pszMsg <= '9') 
						{
                            nPrecision = nPrecision * 10 + (*pszMsg++ - '0');
                        }
                    }
                }

                if (*pszMsg == 'h') 
				{
                    fSmall = TRUE;
                    pszMsg++;
                }
                else if (*pszMsg == 'l' || *pszMsg == 'L') 
				{
                    fLarge = TRUE;
                    pszMsg++;
                }
                else if (*pszMsg == 'I' && pszMsg[1] == '6' && pszMsg[2] == '4') 
				{
                    f64Bit = TRUE;
                    pszMsg += 3;
                }

                if (*pszMsg == 's' || *pszMsg == 'S' ||
                    *pszMsg == 'c' || *pszMsg == 'C')
                {
                    if (*pszMsg == 's' || *pszMsg == 'S')
                    {
                        PCHAR pszDst = szTemp;
                        PVOID pvData = va_arg(args, PVOID);

                        if (*pszMsg == 'S') 
						{
                            fLarge = TRUE;
                        }
                        pszMsg++;

                        fString = TRUE;

                        if (fSmall) 
						{
                            fLarge = FALSE;
                        }

                        __try {

                            if (pvData == NULL) 
							{
                                pszDst = do_str(pszDst, "<NULL>");
                            }
                            else if (fLarge) 
							{
                                pszDst = do_wstr(pszDst, (PWCHAR)pvData);
                            }
                            else 
							{
                                pszDst = do_str(pszDst, (PCHAR)pvData);
                            }
						} __except(EXCEPTION_EXECUTE_HANDLER) {

							pszDst = szTemp;
							szTemp[0] = '\0';
							pszDst = do_str(pszDst, "<");
							pszDst = do_str(pszDst, "EXCEPTION_EXECUTE_HANDLER:");
							pszOut = do_base(pszOut, (UINT64)GetExceptionCode(), 10, "0123456789");
							pszDst = do_str(pszDst, ">");
                            *pszDst = '\0';
						}

/*
						} __except(EXCEPTION_EXECUTE_HANDLER) {

							pszDst = szTemp;
                            *pszDst++ = '<';
                            pszDst = do_base(pszDst, (UINT64)pvData, 16, "0123456789ABCDEF");
                            *pszDst++ = '>';
                            *pszDst = '\0';
						}
*/
						nLen = (int)(pszDst - szTemp);
                    }
                    else {
                        if (*pszMsg == 'S') {
                            fLarge = TRUE;
                        }
                        pszMsg++;

                        fString = TRUE;

                        szTemp[0] = (CHAR)va_arg(args, INT);
                        szTemp[1] = '\0';
                        nLen = 1;
                    }

                    if (nPrecision && nLen > nPrecision) {
                        nLen = nPrecision;
                        szTemp[nLen] = '\0';
                    }

                    if (fLeft) {
                        pszOut = do_str(pszOut, szTemp);
                        for (; nLen < nWidth; nLen++) {
                            *pszOut++ = ' ';
                        }
                    }
                    else {
                        for (; nLen < nWidth; nLen++) {
                            *pszOut++ = ' ';
                        }
                        pszOut = do_str(pszOut, szTemp);
                    }
                }
                else if (*pszMsg == 'd' || *pszMsg == 'i' || *pszMsg == 'o' ||
                         *pszMsg == 'x' || *pszMsg == 'X' || *pszMsg == 'b') 
				{
                    UINT64 value;
                    if (f64Bit) {
                        value = va_arg(args, UINT64);
                    }
                    else 
					{
                        value = va_arg(args, UINT);
                    }

                    if (*pszMsg == 'x') 
					{
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 16, "0123456789abcdef") - szTemp);
                        
						if (fPound && value) 
						{
                            do_str(szHead, "0x");
                        }
                    }
                    else if (*pszMsg == 'X') 
					{
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 16, "0123456789ABCDEF") - szTemp);
                        
						if (fPound && value) 
						{
                            do_str(szHead, "0X");
                        }
                    }
                    else if (*pszMsg == 'd') 
					{
                        pszMsg++;
                        if ((INT64)value < 0) {
                            value = -(INT64)value;
                            do_str(szHead, "-");
                        }
                        else if (fPositive) {
                            if (value > 0) {
                                do_str(szHead, "+");
                            }
                        }
                        else if (fBlank) {
                            if (value > 0) {
                                do_str(szHead, " ");
                            }
                        }
                        nLen = (int)(do_base(szTemp, value, 10, "0123456789") - szTemp);
                        nPrecision = 0;
                    }
                    else if (*pszMsg == 'u') {
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 10, "0123456789") - szTemp);
                        nPrecision = 0;
                    }
                    else if (*pszMsg == 'o') {
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 8, "01234567") - szTemp);
                        nPrecision = 0;

                        if (fPound && value) {
                            do_str(szHead, "0");
                        }
                    }
                    else if (*pszMsg == 'b') {
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 2, "01") - szTemp);
                        nPrecision = 0;

                        if (fPound && value) {
                            do_str(szHead, "0b");
                        }
                    }
                    else {
                        pszMsg++;
                        if ((INT64)value < 0) {
                            value = -(INT64)value;
                            do_str(szHead, "-");
                        }
                        else if (fPositive) {
                            if (value > 0) {
                                do_str(szHead, "+");
                            }
                        }
                        else if (fBlank) {
                            if (value > 0) {
                                do_str(szHead, " ");
                            }
                        }
                        nLen = (int)(do_base(szTemp, value, 10, "0123456789") - szTemp);
                        nPrecision = 0;
                    }

                    INT nHead = 0;

					/* Count characters in head string. */
                    for (; szHead[nHead]; nHead++) {
                    }

                    if (fLeft) {
                        if (nHead) {
                            pszOut = do_str(pszOut, szHead);
                            nLen += nHead;
                        }
                        pszOut = do_str(pszOut, szTemp);
                        for (; nLen < nWidth; nLen++) {
                            *pszOut++ = ' ';
                        }
                    }
                    else if (fZero) {
                        if (nHead) {
                            pszOut = do_str(pszOut, szHead);
                            nLen += nHead;
                        }
                        for (; nLen < nWidth; nLen++) {
                            *pszOut++ = '0';
                        }
                        pszOut = do_str(pszOut, szTemp);
                    }
                    else {
                        if (nHead) {
                            nLen += nHead;
                        }
                        for (; nLen < nWidth; nLen++) {
                            *pszOut++ = ' ';
                        }
                        if (nHead) {
                            pszOut = do_str(pszOut, szHead);
                        }
                        pszOut = do_str(pszOut, szTemp);
                    }
                }
                else if (*pszMsg == 'p' || *pszMsg == 'P') {
                    ULONG_PTR value;
                    value = va_arg(args, ULONG_PTR);

                    if (*pszMsg == 'p') {
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 16, "0123456789abcdef") - szTemp);
                        if (fPound && value) {
                            do_str(szHead, "0x");
                        }
                    }
                    else {
                        pszMsg++;
                        nLen = (int)(do_base(szTemp, value, 16, "0123456789ABCDEF") - szTemp);
                        if (fPound && value) {
                            do_str(szHead, "0x");
                        }
                    }

                    INT nHead = 0;

                    /* Count characters in head string. */
                    for (; szHead[nHead]; nHead++) {
                    }

                    if (nHead) {
                        pszOut = do_str(pszOut, szHead);
                        nLen += nHead;
                    }
                    for (; nLen < nWidth; nLen++) {
                        *pszOut++ = '0';
                    }
                    pszOut = do_str(pszOut, szTemp);
                }
                else {
                    pszMsg++;
                    while (pszArg < pszMsg) {
                        *pszOut++ = *pszArg++;
                    }
                }
            }
            else {
                *pszOut++ = *pszMsg++;
            }
		}

		*pszOut = '\0';
		pszBuffer[SERVICESNAPSHOT_TRAMPOLINE_BUFFERSIZE - 1] = '\0';

	} __except(EXCEPTION_EXECUTE_HANDLER) {

		pszOut = pszBuffer;
		pszBuffer[0] = '\0';
        pszOut = do_str(pszOut, "EXCEPTION_EXECUTE_HANDLER|");
        pszOut = do_base(pszOut, (UINT64)GetExceptionCode(), 10, "0123456789");
		*pszOut = '\0';
	}
/*
    } __except(EXCEPTION_EXECUTE_HANDLER) {
		PCHAR pszOut = pszBuffer;
        *pszOut = '\0';
        pszOut = do_str(pszOut, "EXCEPTION_EXECUTE_HANDLER|");
        pszOut = do_base(pszOut, (UINT64)GetExceptionCode(), 10, "0123456789");
	}
*/
	/* Pipe the string to the daemon. */
	_Pipe(pszBuffer);
}
PCHAR do_base(PCHAR pszOut, UINT64 nValue, UINT nBase, PCSTR pszDigits)
{
    CHAR szTmp[96];
    int nDigit = sizeof(szTmp)-2;
    for (; nDigit >= 0; nDigit--) {
        szTmp[nDigit] = pszDigits[nValue % nBase];
        nValue /= nBase;
    }
    for (nDigit = 0; nDigit < sizeof(szTmp) - 2 && szTmp[nDigit] == '0'; nDigit++) {
        /* Skip leading zeros. */
    }
    for (; nDigit < sizeof(szTmp) - 1; nDigit++) {
        *pszOut++ = szTmp[nDigit];
    }
    *pszOut = '\0';
    return pszOut;
}
PCHAR do_str(PCHAR pszOut, PCSTR pszIn)
{
    while (*pszIn) {
        *pszOut++ = *pszIn++;
    }
    *pszOut = '\0';
    return pszOut;
}

PCHAR do_wstr(PCHAR pszOut, PCWSTR pszIn)
{
    while (*pszIn) {
		/* "Crazy" cast the WCHAR to CHAR. */
		*pszOut++ = (*pszIn++ & 0xFF);
//		*pszOut++ = (CHAR)*pszIn++;
    }
    *pszOut = '\0';
    return pszOut;
}
INT _EvalException(INT n_except) {
	/* Pass on most exceptions. */
	if ( n_except != STATUS_INTEGER_OVERFLOW && n_except != STATUS_FLOAT_OVERFLOW )
	{
		return EXCEPTION_CONTINUE_SEARCH;
	}
	/* Execute some code to clean up problem. */
	else
	{
//	ResetVars( 0 );   // initializes data to 0
		return EXCEPTION_CONTINUE_EXECUTION;
//		return EXCEPTION_CONTINUE_SEARCH;
	}
}