//#define ENABLE_LOG
/*
 * Copyright (c) 2014 - 2022 t_saki@serenegiant.com 
 */

using System;
using System.Runtime.InteropServices;

/*
 * THETA S  vid:1482, pid:1001
 * THETA V  vid:1482, pid:1002
 * THETA Z1 vid:1482, pid:1005
 */

namespace Serenegiant.UVC
{

	[Serializable]
	public class UVCDevice
	{
		private readonly IntPtr ptr;
		public readonly Int32 id;
		public readonly int vid;
		public readonly int pid;
		public readonly int deviceClass;
		public readonly int deviceSubClass;
		public readonly int deviceProtocol;

		public readonly string name;

		public UVCDevice(IntPtr devicePtr) {
			ptr = devicePtr;
			id = GetId(devicePtr);
			vid = GetVendorId(devicePtr);
			pid = GetProductId(devicePtr);
			name = GetName(devicePtr);
			deviceClass = GetDeviceClass(devicePtr);
			deviceSubClass = GetDeviceSubClass(devicePtr);
			deviceProtocol = GetDeviceProtocol(devicePtr);
		}

		public override string ToString()
		{
			return $"{base.ToString()}(id={id},vid={vid},pid={pid},name={name},deviceClass={deviceClass},deviceSubClass={deviceSubClass},deviceProtocol={deviceProtocol})";
		}


		/**
		 * Ricohの製品かどうか
		 * Is it a Ricoh product?
		 * @param info
		 */
		public bool IsRicoh
		{
			get { return (vid == 1482); }
		}

        /**
		 * THETA S/V/Z1のいずれかかどうか
		 */
        public bool IsTHETA
        {
            get { return IsTHETA_S || IsTHETA_V || IsTHETA_Z1; }
        }
   
		/**
		 * THETA Sかどうか
		 */
		public bool IsTHETA_S
		{
			get { return (vid == 1482) && (pid == 10001); }
		}

		/**
		 * THETA Vかどうか
		 */
		public bool IsTHETA_V
		{
			// THETA Vからのpid=872はUVCでなくて動かないので注意(THETA側は静止画/動画モード)
			// Note that pid=872 from THETA V does not work unless it is UVC (THETA side is still image/video mode)
			get { return (vid == 1482) && (pid == 10002); }
		}

        /**
		 * THETA Z1かどうか
		 * @param info
		 */
        public bool IsTHETA_Z1
        {
            // THETA Z1からのpid=877はUVCではなくて動かないので注意(THETA側は静止画/動画モード)
            // Note that pid=877 from THETA Z1 is not UVC and does not move (THETA side is still image/video mode)
            get { return (vid == 1482) && (pid == 10005); }
        }

		/**
		 * UACに対応しているかどうか
		 * XXX UACに対応していると応答するUVC機器でも実際にはUACに未対応なバギーな機器も存在するので注意！
		 * Whether or not UAC is supported
		 * Note that even UVC devices that respond that they support XXX UAC are buggy devices that do not actually support UAC!
		 */
		public bool isUAC
		{
			get { return Match(1, 1, 0xff) && Match(1, 2, 0xff); }
		}

		/**
		 * デバイスまたはインターフェースが指定した条件に一致するかどうかを確認
		 * Verify that a device or interface matches the specified criteria
		 * @param bClass 0xffなら常にマッチする(ワイルドカード)
		 * @param bClass 0xff If so, always match (wildcard)
		 * @param bSubClass 0xffなら常にマッチする(ワイルドカード)
		 * @param bSubClass 0xff If so, always match (wildcard)
		 * @param bProtocol 0xffなら常にマッチする(ワイルドカード)
		 * @param bProtocol 0xff If so, always match (wildcard)
		 * @returtn 1: 一致した, 0: 一致しなかった
		 * @returtn 1: Matched, 0: Not matched
		 */
		public bool Match(byte bClass, byte bSubClass, byte bProtocol)
		{
			var result = InternalMatch(ptr, bClass, bSubClass, bProtocol);
			return result != 0;
		}

		//--------------------------------------------------------------------------------
		// プラグインのインターフェース関数
		// Plug-in Interface Functions
		//--------------------------------------------------------------------------------
		/**
		 * 機器idを取得(これだけはpublicにする)
		 * Get the device ID (only make it public)
		 */
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_id")]
		public static extern Int32 GetId(IntPtr devicePtr);

		/**
			* デバイスクラスを取得
			* Get Device Class
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_device_class")]
		private static extern Byte GetDeviceClass(IntPtr devicePtr);

		/**
			* デバイスサブクラスを取得
			* Get Device Subclass
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_device_sub_class")]
		private static extern Byte GetDeviceSubClass(IntPtr devicePtr);

		/**
			* デバイスプロトコルを取得
			* Get Device Protocol
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_device_protocol")]
		private static extern Byte GetDeviceProtocol(IntPtr devicePtr);

		/**
			* ベンダーIDを取得
			* Get Vendor ID
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_vendor_id")]
		private static extern UInt16 GetVendorId(IntPtr devicePtr);

		/**
			* プロダクトIDを取得
			* Get Product ID
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_product_id")]
		private static extern UInt16 GetProductId(IntPtr devicePtr);

		/**
			* 機器名を取得
			* Get device name
			*/
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_get_name")]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private static extern string GetName(IntPtr devicePtr);


		/**
		 * デバイスまたはインターフェースが指定した条件に一致するかどうかを確認
		 * Verify that a device or interface matches the specified criteria
		 * @param device
		 * @param bClass 0xffなら常にマッチする(ワイルドカード)
		 * @param bClass 0xff If so, always match (wildcard)
		 * @param bSubClass 0xffなら常にマッチする(ワイルドカード)
		 * @param bSubClass 0xff If so, always match (wildcard)
		 * @param bProtocol 0xffなら常にマッチする(ワイルドカード)
		 * @param bProtocol 0xff If so, always match (wildcard)
		 * @returtn 1: 一致した, 0: 一致しなかった
		 * @returtn 1: Matched, 0: Not matched
		 */
		[DllImport("unityuvcplugin", EntryPoint = "DeviceInfo_match")]
		private static extern Int32 InternalMatch(IntPtr devicePtr, byte bClass, byte bSubClass, byte bProtocol);
	} // UVCDevice

} // namespace Serenegiant.UVC

