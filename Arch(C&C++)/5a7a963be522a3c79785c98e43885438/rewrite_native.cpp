#include "stdafx.h"
#include "rewrite_native.h"

BOOL CALLBACK ExportCallback(PVOID pContext, ULONG nOrdinal, PCHAR pszSymbol, PVOID pbTarget)
{
    (void)pContext;
    (void)pbTarget;
    (void)pszSymbol;

    if (nOrdinal == 1) {
        *((BOOL *)pContext) = TRUE;
    }
    return TRUE;
}
