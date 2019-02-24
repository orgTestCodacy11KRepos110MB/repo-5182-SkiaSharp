﻿using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace HarfBuzzSharp
{
	public class Buffer : NativeObject
	{
		public const int DefaultReplacementCodepoint = '\uFFFD';

		public Buffer ()
			: this (HarfBuzzApi.hb_buffer_create ())
		{
		}

		internal Buffer (IntPtr handle)
			: base (handle)
		{
			Language = new Language (CultureInfo.CurrentCulture);
		}

		public ContentType ContentType {
			get { return HarfBuzzApi.hb_buffer_get_content_type (Handle); }
			set { HarfBuzzApi.hb_buffer_set_content_type (Handle, value); }
		}

		public Direction Direction {
			get { return HarfBuzzApi.hb_buffer_get_direction (Handle); }
			set { HarfBuzzApi.hb_buffer_set_direction (Handle, value); }
		}

		public Language Language {
			get { return new Language (HarfBuzzApi.hb_buffer_get_language (Handle)); }
			set { HarfBuzzApi.hb_buffer_set_language (Handle, value.Handle); }
		}

		public Flags Flags {
			get { return HarfBuzzApi.hb_buffer_get_flags (Handle); }
			set { HarfBuzzApi.hb_buffer_set_flags (Handle, value); }
		}

		public ClusterLevel ClusterLevel {
			get { return HarfBuzzApi.hb_buffer_get_cluster_level (Handle); }
			set { HarfBuzzApi.hb_buffer_set_cluster_level (Handle, value); }
		}

		public int ReplacementCodepoint {
			get { return HarfBuzzApi.hb_buffer_get_replacement_codepoint (Handle); }
			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException (nameof (value), "Value must be non negative.");
				}
				HarfBuzzApi.hb_buffer_set_replacement_codepoint (Handle, value);
			}
		}

		public Script Script {
			get { return HarfBuzzApi.hb_buffer_get_script (Handle); }
			set { HarfBuzzApi.hb_buffer_set_script (Handle, value); }
		}

		public int Length {
			get { return HarfBuzzApi.hb_buffer_get_length (Handle); }
			set {
				if (value < 0) {
					throw new ArgumentOutOfRangeException (nameof (value), "Value must be non negative.");
				}
				HarfBuzzApi.hb_buffer_set_length (Handle, value);
			}
		}

		public void Add (int codepoint, int cluster)
		{
			if (codepoint < 0) {
				throw new ArgumentOutOfRangeException (nameof (codepoint), "Value must be non negative.");
			}

			if (cluster < 0) {
				throw new ArgumentOutOfRangeException (nameof (codepoint), "Value must be non negative.");
			}

			if (Length != 0 && ContentType != ContentType.Unicode) {
				throw new InvalidOperationException ("Non empty buffer's ContentType must be of type Unicode.");
			}

			if (ContentType == ContentType.Glyphs) {
				throw new InvalidOperationException ("ContentType must not be of type Glyphs");
			}

			HarfBuzzApi.hb_buffer_add (Handle, codepoint, cluster);
		}

		public void AddUtf8 (string utf8text)
		{
			var bytes = Encoding.UTF8.GetBytes (utf8text);
			AddUtf8 (bytes, 0, -1);
		}

		public unsafe void AddUtf8 (byte[] bytes, int itemOffset = 0, int itemLength = -1)
		{
			fixed (byte* b = bytes) {
				AddUtf8 ((IntPtr)b, bytes.Length, itemOffset, itemLength);
			}
		}

		public unsafe void AddUtf8 (ReadOnlySpan<byte> text, int itemOffset = 0, int itemLength = -1)
		{
			fixed (byte* bytes = text) {
				AddUtf8 ((IntPtr)bytes, text.Length, itemOffset, itemLength);
			}
		}

		public void AddUtf8 (IntPtr text, int textLength, int itemOffset = 0, int itemLength = -1)
		{
			if (itemOffset < 0) {
				throw new ArgumentOutOfRangeException (nameof (itemOffset), "Value must be non negative.");
			}

			if (Length != 0 && ContentType != ContentType.Unicode) {
				throw new InvalidOperationException ("Non empty buffer's ContentType must be of type Unicode.");
			}

			if (ContentType == ContentType.Glyphs) {
				throw new InvalidOperationException ("ContentType must not be Glyphs");
			}

			HarfBuzzApi.hb_buffer_add_utf8 (Handle, text, textLength, itemOffset, itemLength);
		}

		public unsafe void AddUtf16 (string text, int itemOffset = 0, int itemLength = -1)
		{
			fixed (char* chars = text) {
				AddUtf16 ((IntPtr)chars, text.Length, itemOffset, itemLength);
			}
		}

		public unsafe void AddUtf16 (byte[] text)
		{
			fixed (byte* bytes = text) {
				AddUtf16 ((IntPtr)bytes, text.Length / 2);
			}
		}

		public unsafe void AddUtf16 (ReadOnlySpan<char> text, int itemOffset = 0, int itemLength = -1)
		{
			fixed (char* chars = text) {
				AddUtf16 ((IntPtr)chars, text.Length, itemOffset, itemLength);
			}
		}

		public void AddUtf16 (IntPtr text, int textLength, int itemOffset = 0, int itemLength = -1)
		{
			if (itemOffset < 0) {
				throw new ArgumentOutOfRangeException (nameof (itemOffset), "Value must be non negative.");
			}

			if (Length != 0 && ContentType != ContentType.Unicode) {
				throw new InvalidOperationException ("Non empty buffer's ContentType must be of type Unicode.");
			}

			if (ContentType == ContentType.Glyphs) {
				throw new InvalidOperationException ("ContentType must not be of type Glyphs");
			}

			HarfBuzzApi.hb_buffer_add_utf16 (Handle, text, textLength, itemOffset, itemLength);
		}

		public void AddUtf32 (string text)
		{
			var bytes = Encoding.UTF32.GetBytes (text);
			AddUtf32 (bytes);
		}

		public unsafe void AddUtf32 (byte[] text)
		{
			fixed (byte* bytes = text) {
				AddUtf32 ((IntPtr)bytes, text.Length / 4);
			}
		}

		public unsafe void AddUtf32 (ReadOnlySpan<int> text, int itemOffset = 0, int itemLength = -1)
		{
			fixed (int* integers = text) {
				AddUtf32 ((IntPtr)integers, text.Length, itemOffset, itemLength);
			}
		}

		public void AddUtf32 (IntPtr text, int textLength, int itemOffset = 0, int itemLength = -1)
		{
			if (itemOffset < 0) {
				throw new ArgumentOutOfRangeException (nameof (itemOffset), "Value must be non negative.");
			}

			if (Length != 0 && ContentType != ContentType.Unicode) {
				throw new InvalidOperationException("Non empty buffer's ContentType must be of type Unicode.");
			}

			if (ContentType == ContentType.Glyphs) {
				throw new InvalidOperationException("ContentType must not be of type Glyphs");
			}

			HarfBuzzApi.hb_buffer_add_utf32 (Handle, text, textLength, itemOffset, itemLength);
		}

		public void GuessSegmentProperties () => HarfBuzzApi.hb_buffer_guess_segment_properties (Handle);

		public void ClearContents () => HarfBuzzApi.hb_buffer_clear_contents (Handle);

		public void Reset () => HarfBuzzApi.hb_buffer_reset (Handle);

		public GlyphInfo[] GlyphInfos {
			get {
				var infoPtrs = HarfBuzzApi.hb_buffer_get_glyph_infos (Handle, out var length);
				return PtrToStructureArray<GlyphInfo> (infoPtrs, (int)length);
			}
		}

		public GlyphPosition[] GlyphPositions {
			get {
				var infoPtrs = HarfBuzzApi.hb_buffer_get_glyph_positions (Handle, out var length);
				return PtrToStructureArray<GlyphPosition> (infoPtrs, (int)length);
			}
		}

		public string SerializeGlyphs (
			int start = 0,
			int end = -1,
			Font font = null,
			SerializeFormat format = SerializeFormat.Text,
			SerializeFlag flags = SerializeFlag.Default)
		{
			const uint bufferSize = 128;

			if (Length == 0) {
				throw new InvalidOperationException ("Buffer should not be empty.");
			}

			if (ContentType != ContentType.Glyphs) {
				throw  new InvalidOperationException("ContentType should be of type Glyphs.");
			}

			if (end == -1) {
				end = Length;
			}

			var builder = new StringBuilder (128);
			var buffer = Marshal.AllocHGlobal ((int)bufferSize);
			var currentPosition = start;

			try {
				while (currentPosition < end) {
					currentPosition += HarfBuzzApi.hb_buffer_serialize_glyphs (
						Handle,
						currentPosition,
						end,
						buffer,
						bufferSize,
						out var consumed,
						font?.Handle ?? IntPtr.Zero,
						format,
						flags);

					builder.Append (Marshal.PtrToStringAnsi (buffer, (int)consumed));
				}

			} finally {
				Marshal.FreeHGlobal (buffer);
			}

			return builder.ToString ();
		}

		public void DeSerializeGlyphs (string data, Font font = null, SerializeFormat format = SerializeFormat.Text)
		{
			if (Length != 0) {
				throw new InvalidOperationException("Buffer must be empty.");
			}

			if (ContentType == ContentType.Glyphs) {
				throw new InvalidOperationException("ContentType must not be Glyphs.");
			}

			HarfBuzzApi.hb_buffer_deserialize_glyphs (Handle, data, -1, out _, font?.Handle ?? IntPtr.Zero, format);
		}
		protected override void Dispose (bool disposing)
		{
			if (Handle != IntPtr.Zero) {
				HarfBuzzApi.hb_buffer_destroy (Handle);
			}

			base.Dispose (disposing);
		}
	}
}
