#define ENABLE_LOG
#define DEBUG
#define CONSOLE_REDIRECT



using UnityEngine;
namespace Serenegiant.UVC
{
	public static class Console {

#if (!NDEBUG && DEBUG && ENABLE_LOG && CONSOLE_REDIRECT)
		public static void WriteLine(string msg) {
			Debug.Log(msg);
		}
#endif
	}

}   // namespace Serenegiant.UVC
