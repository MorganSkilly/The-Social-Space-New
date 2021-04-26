using UnityEngine;
using System;
using System.Collections.Generic;

//-----------------------------------------------------------------------------
// Copyright 2015-2021 RenderHeads Ltd.  All rights reserved.
//-----------------------------------------------------------------------------

namespace RenderHeads.Media.AVProVideo
{
	public partial class MediaPlayer : MonoBehaviour
	{

#region PlatformOptions
		[System.Serializable]
		public class PlatformOptions
		{
			public virtual bool IsModified()
			{
				return (httpHeaders.IsModified()
				|| keyAuth.IsModified()
				);
			}

			public HttpHeaderData httpHeaders = new HttpHeaderData();
			public KeyAuthData keyAuth = new KeyAuthData();

			// Decryption support
			public virtual string GetKeyServerAuthToken() { return keyAuth.keyServerToken; }
			//public virtual string GetKeyServerURL() { return null; }
			public virtual byte[] GetOverrideDecryptionKey() { return keyAuth.overrideDecryptionKey; }

			public virtual bool StartWithHighestBandwidth() { return false; }
		}

		[System.Serializable]
		public class OptionsWindows : PlatformOptions, ISerializationCallbackReceiver
		{
			public Windows.VideoApi videoApi = Windows.VideoApi.MediaFoundation;
			public bool useHardwareDecoding = true;
			public bool useTextureMips = false;
			public bool use10BitTextures = false;
			public bool hintAlphaChannel = false;
			public bool useLowLatency = false;
			public bool useCustomMovParser = false;
			public string forceAudioOutputDeviceName = string.Empty;
			public List<string> preferredFilters = new List<string>();
			public Windows.AudioOutput audioOutput = Windows.AudioOutput.System;
			public Audio360ChannelMode audio360ChannelMode = Audio360ChannelMode.TBE_8_2;
			public bool startWithHighestBitrate = false;

			/// Hap & NotchLC only
			[Range(1, 16)]
			public int parallelFrameCount = 3;
			/// Hap & NotchLC only
			[Range(1, 16)]
			public int prerollFrameCount = 4;

			public override bool IsModified()
			{
				return (base.IsModified()
				|| !useHardwareDecoding
				|| useTextureMips
				|| use10BitTextures
				|| hintAlphaChannel
				|| useLowLatency
				|| useCustomMovParser
				|| videoApi != Windows.VideoApi.MediaFoundation
				|| audioOutput != Windows.AudioOutput.System
				|| audio360ChannelMode != Audio360ChannelMode.TBE_8_2
				|| !string.IsNullOrEmpty(forceAudioOutputDeviceName)
				|| preferredFilters.Count != 0
				|| startWithHighestBitrate
				|| parallelFrameCount != 3
				|| prerollFrameCount != 4
				);
			}

			public override bool StartWithHighestBandwidth() { return startWithHighestBitrate; }

			#region Upgrade from Version 1.x
			[SerializeField, HideInInspector]
			private bool useUnityAudio = false;
			[SerializeField, HideInInspector]
			private bool enableAudio360 = false;

			void ISerializationCallbackReceiver.OnBeforeSerialize() { }

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				if (useUnityAudio && audioOutput == Windows.AudioOutput.System)
				{
					audioOutput = Windows.AudioOutput.Unity;
					useUnityAudio = false;
				}
				if (enableAudio360 && audioOutput == Windows.AudioOutput.System)
				{
					audioOutput = Windows.AudioOutput.FacebookAudio360;
					enableAudio360 = false;
				}
			}
			#endregion	// Upgrade from Version 1.x
		}

		[System.Serializable]
		public class OptionsWindowsUWP : PlatformOptions
		{
			public bool useHardwareDecoding = true;
			public bool useTextureMips = false;
			public bool use10BitTextures = false;
			public bool hintOutput10Bit = false;
			public bool useLowLatency = false;
			public WindowsUWP.VideoApi videoApi = WindowsUWP.VideoApi.WinRT;
			public WindowsUWP.AudioOutput audioOutput = WindowsUWP.AudioOutput.System;
			public Audio360ChannelMode audio360ChannelMode = Audio360ChannelMode.TBE_8_2;

			public bool startWithHighestBitrate = false;

			public override bool IsModified()
			{
				return (base.IsModified()
				|| !useHardwareDecoding
				|| useTextureMips
				|| use10BitTextures
				|| useLowLatency
				|| audioOutput != WindowsUWP.AudioOutput.System
				|| (audio360ChannelMode != Audio360ChannelMode.TBE_8_2)
				|| videoApi != WindowsUWP.VideoApi.WinRT
				|| startWithHighestBitrate
				);
			}

			public override bool StartWithHighestBandwidth() { return startWithHighestBitrate; }
		}

		[System.Serializable]
		public class OptionsApple: PlatformOptions
		{
			public enum TextureFormat: int
			{
				BGRA,
				YCbCr420,
			}

			public enum AudioMode
			{
				SystemDirect,
				Unity
			};

			[Flags]
			public enum Flags: int
			{
				// Common
				None = 0,
				GenerateMipMaps = 1 << 0,

				// iOS & macOS
				AllowExternalPlayback = 1 << 8,

				// iOS
				ResumeMediaPlaybackAfterAudioSessionRouteChange = 1 << 16,
			}

			public enum Resolution
			{
				NoPreference,
				_480p,
				_720p,
				_1080p,
				_1440p,
				_2160p,
				Custom
			}

			public enum BitRateUnits
			{
				bps,
				Kbps,
				Mbps,
			}

			private readonly TextureFormat DefaultTextureFormat;
			private readonly Flags DefaultFlags;
			public TextureFormat textureFormat;
			public AudioMode audioMode;
			public Flags flags;
			public float maximumPlaybackRate = 2.0f;
			public Resolution preferredMaximumResolution = Resolution.NoPreference;
			#if UNITY_2017_2_OR_NEWER
			public Vector2Int customPreferredMaximumResolution = Vector2Int.zero;
			#endif
			public float preferredPeakBitRate = 0.0f;
			public BitRateUnits preferredPeakBitRateUnits = BitRateUnits.Kbps;

			private static double BitRateInBitsPerSecond(float value, BitRateUnits units)
			{
				switch (units)
				{
					case BitRateUnits.bps:
						return (double)value;
					case BitRateUnits.Kbps:
						return (double)value * 1000.0;
					case BitRateUnits.Mbps:
						return (double)value * 1000000.0;
					default:
						return 0.0;
				}
			}

			public double GetPreferredPeakBitRateInBitsPerSecond()
			{
				return BitRateInBitsPerSecond(preferredPeakBitRate, preferredPeakBitRateUnits);
			}

			public OptionsApple(TextureFormat defaultTextureFormat, Flags defaultFlags)
			{
				DefaultTextureFormat = defaultTextureFormat;
				DefaultFlags = defaultFlags;
				textureFormat = defaultTextureFormat;
				audioMode = AudioMode.SystemDirect;
				flags = defaultFlags;
			}

			public override bool IsModified()
			{
				return base.IsModified()
					|| textureFormat != DefaultTextureFormat
					|| audioMode != AudioMode.SystemDirect
					|| flags != DefaultFlags
					|| preferredMaximumResolution != Resolution.NoPreference
					|| preferredPeakBitRate != 0;
			}
		}

		[System.Serializable]
		public class OptionsAndroid : PlatformOptions, ISerializationCallbackReceiver
		{
			public enum Resolution
			{
				NoPreference,
				_480p,
				_720p,
				_1080p,
				_2160p,
				Custom
			}

			public Resolution preferredMaximumResolution = Resolution.NoPreference;
#if UNITY_2017_2_OR_NEWER
			public Vector2Int customPreferredMaximumResolution = Vector2Int.zero;
#endif

			public Android.VideoApi videoApi = Android.VideoApi.ExoPlayer;
			public bool useFastOesPath = false;
			public bool showPosterFrame = false;
			public Android.AudioOutput audioOutput = Android.AudioOutput.System;
			public Audio360ChannelMode audio360ChannelMode = Audio360ChannelMode.TBE_8_2;
			public bool preferSoftwareDecoder = false;

			[SerializeField, Tooltip("Byte offset into the file where the media file is located.  This is useful when hiding or packing media files within another file.")]
			public int fileOffset = 0;

			public bool startWithHighestBitrate = false;

			public int minBufferMs							= Android.Default_MinBufferTimeMs;
			public int maxBufferMs							= Android.Default_MaxBufferTimeMs;
			public int bufferForPlaybackMs					= Android.Default_BufferForPlaybackMs;
			public int bufferForPlaybackAfterRebufferMs		= Android.Default_BufferForPlaybackAfterRebufferMs;

			public override bool IsModified()
			{
				return (base.IsModified()
					|| (fileOffset != 0)
					|| useFastOesPath
					|| showPosterFrame
					|| (videoApi != Android.VideoApi.ExoPlayer)
					|| audioOutput != Android.AudioOutput.System
					|| (audio360ChannelMode != Audio360ChannelMode.TBE_8_2)
					|| preferSoftwareDecoder
					|| startWithHighestBitrate
					|| (minBufferMs != Android.Default_MinBufferTimeMs)
					|| (maxBufferMs != Android.Default_MaxBufferTimeMs)
					|| (bufferForPlaybackMs != Android.Default_BufferForPlaybackMs)
					|| (bufferForPlaybackAfterRebufferMs != Android.Default_BufferForPlaybackAfterRebufferMs)
					|| (preferredMaximumResolution != Resolution.NoPreference)
				);
			}

			public override bool StartWithHighestBandwidth() { return startWithHighestBitrate; }

			#region Upgrade from Version 1.x
			[SerializeField, HideInInspector]
			private bool enableAudio360 = false;

			void ISerializationCallbackReceiver.OnBeforeSerialize()	{ }

			void ISerializationCallbackReceiver.OnAfterDeserialize()
			{
				if (enableAudio360 && audioOutput == Android.AudioOutput.System)
				{
					audioOutput = Android.AudioOutput.FacebookAudio360;
					enableAudio360 = false;
				}
			}
			#endregion	// Upgrade from Version 1.x
		}

		[System.Serializable]
		public class OptionsWebGL : PlatformOptions
		{
			public WebGL.ExternalLibrary externalLibrary = WebGL.ExternalLibrary.None;
			public bool useTextureMips = false;

			public override bool IsModified()
			{
				return (base.IsModified() || externalLibrary != WebGL.ExternalLibrary.None || useTextureMips);
			}

			// Decryption support
			public override string GetKeyServerAuthToken() { return null; }
			public override byte[] GetOverrideDecryptionKey() { return null; }
		}

		// TODO: move these to a Setup object
		[SerializeField] OptionsWindows _optionsWindows = new OptionsWindows();
		[SerializeField] OptionsApple _optionsMacOSX = new OptionsApple(OptionsApple.TextureFormat.BGRA, OptionsApple.Flags.None);
		[SerializeField] OptionsApple _optionsIOS = new OptionsApple(OptionsApple.TextureFormat.BGRA, OptionsApple.Flags.None);
		[SerializeField] OptionsApple _optionsTVOS = new OptionsApple(OptionsApple.TextureFormat.BGRA, OptionsApple.Flags.None);
		[SerializeField] OptionsAndroid _optionsAndroid = new OptionsAndroid();
		[SerializeField] OptionsWindowsUWP _optionsWindowsUWP = new OptionsWindowsUWP();
		[SerializeField] OptionsWebGL _optionsWebGL = new OptionsWebGL();

		public OptionsWindows PlatformOptionsWindows { get { return _optionsWindows; } }
		public OptionsApple PlatformOptionsMacOSX { get { return _optionsMacOSX; } }
		public OptionsApple PlatformOptionsIOS { get { return _optionsIOS; } }
		public OptionsApple PlatformOptionsTVOS { get { return _optionsTVOS; } }
		public OptionsAndroid PlatformOptionsAndroid { get { return _optionsAndroid; } }
		public OptionsWindowsUWP PlatformOptionsWindowsUWP { get { return _optionsWindowsUWP; } }
		public OptionsWebGL PlatformOptionsWebGL { get { return _optionsWebGL; } }

#endregion // PlatformOptions
	}

#region PlatformOptionsExtensions
	public static class OptionsAppleExtensions
	{
		public static bool GenerateMipmaps(this MediaPlayer.OptionsApple.Flags flags)
		{
			return (flags & MediaPlayer.OptionsApple.Flags.GenerateMipMaps) == MediaPlayer.OptionsApple.Flags.GenerateMipMaps;
		}

		public static MediaPlayer.OptionsApple.Flags SetGenerateMipMaps(this MediaPlayer.OptionsApple.Flags flags, bool b)
		{
			if (flags.GenerateMipmaps() ^ b)
			{
				flags = b ? flags | MediaPlayer.OptionsApple.Flags.GenerateMipMaps
				          : flags & ~MediaPlayer.OptionsApple.Flags.GenerateMipMaps;
			}
			return flags;
		}

		public static bool AllowExternalPlayback(this MediaPlayer.OptionsApple.Flags flags)
		{
			return (flags & MediaPlayer.OptionsApple.Flags.AllowExternalPlayback) == MediaPlayer.OptionsApple.Flags.AllowExternalPlayback;
		}

		public static MediaPlayer.OptionsApple.Flags SetAllowExternalPlayback(this MediaPlayer.OptionsApple.Flags flags, bool b)
		{
			if (flags.ResumePlaybackAfterAudioSessionRouteChange() ^ b)
			{
				flags = b ? flags | MediaPlayer.OptionsApple.Flags.AllowExternalPlayback
				          : flags & ~MediaPlayer.OptionsApple.Flags.AllowExternalPlayback;
			}
			return flags;
		}

		public static bool ResumePlaybackAfterAudioSessionRouteChange(this MediaPlayer.OptionsApple.Flags flags)
		{
			return (flags & MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange) == MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange;
		}

		public static MediaPlayer.OptionsApple.Flags SetResumePlaybackAfterAudioSessionRouteChange(this MediaPlayer.OptionsApple.Flags flags, bool b)
		{
			if (flags.ResumePlaybackAfterAudioSessionRouteChange() ^ b)
			{
				flags = b ? flags | MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange
				          : flags & ~MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange;
			}
			return flags;
		}
	}
#endregion // PlatformOptionsExtensions
}
