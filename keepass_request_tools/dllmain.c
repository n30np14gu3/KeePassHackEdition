#define bool unsigned char
#define true 1
#define false 0

bool __stdcall DllEp(
    void* hinstDLL,
    unsigned fdwReason,   
    void* lpvReserved)  
{
    // Perform actions based on the reason for calling.
    switch (fdwReason)
    {
    case 1:
        return true;

    case 2:
    case 3:
    case 0:
        break;
    }
    return true;
}