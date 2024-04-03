/*
 * Copyright (c) 2014 - 2022 t_saki@serenegiant.com 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Serenegiant.UVC
{

	/**
	 * UVC関係のイベントハンドリングインターフェース
	 * UVC-related event handling interface
	 */
	public interface IUVCDrawer
	{
		/**
		 * UVC機器が接続された
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * @return true: UVC機器を使う, false: UVC機器を使わない
		 * UVC equipment connected
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 * @return true: Use UVC devices, false: Do not use UVC devices
		 */
		bool OnUVCAttachEvent(UVCManager manager, UVCDevice device);
		/**
		 * UVC機器が取り外された
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * UVC equipment removed
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 */
		void OnUVCDetachEvent(UVCManager manager, UVCDevice device);
		/**
		 * IUVCDrawerが指定したUVC機器の映像を描画できるかどうかを取得
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * Gets whether IUVC Drawer can draw images of the specified UVC device.
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 */
		bool IsUVCEnabled(UVCManager manager, UVCDevice device);
		/**
		 * UVC機器からの映像取得を開始した
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * @param tex UVC機器からの映像を受け取るTextureオブジェクト
		 * Started acquiring images from UVC devices
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 * @param tex A Texture object that receives the image from the UVC device
		 */
		void OnUVCStartEvent(UVCManager manager, UVCDevice device, Texture tex);
		/**
		 * UVC機器からの映像取得を終了した
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * Video acquisition from the UVC device has been terminated.
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 */
		void OnUVCStopEvent(UVCManager manager, UVCDevice device);

		/**
		 * IUVCDrawerが指定したUAC機器の音声を取得できるかどうかを取得
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUAC機器情報
		 * Gets whether IUVC Drawer can acquire the audio of the specified UAC device.
		 * @param manager Calling UVCManager
		 * @param device Connected UAC device information
		 */
		bool IsUACEnabled(UVCManager manager, UVCDevice device);
		/**
		 * UAC機器からの音声取得を開始した
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * @param audioClip UAC機器からの音声を受け取るAudioClipオブジェクト
		 * Starts acquiring audio from a UAC device.
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 * @param audioClip An Audio Clip object that receives audio from a UAC device
		 */
		void OnUACStartEvent(UVCManager manager, UVCDevice device, AudioClip audioClip);
		/**
		 * UAC機器からの音声取得を終了した
		 * @param manager 呼び出し元のUVCManager
		 * @param device 接続されたUVC機器情報
		 * Audio acquisition from the UAC device has been terminated.
		 * @param manager Calling UVCManager
		 * @param device Connected UVC device information
		 */
		void OnUACStopEvent(UVCManager manager, UVCDevice device);
	}   // interface IUVCDrawer

}	// namespace Serenegiant.UVC
