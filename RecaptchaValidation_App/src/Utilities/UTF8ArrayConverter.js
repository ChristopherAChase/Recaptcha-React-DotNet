const fromUTF8Array = (data) => {
  // array of bytes
  var str = "",
    i;

  for (i = 0; i < data.length; i++) {
    var value = data[i];

    if (value < 0x80) {
      str += String.fromCharCode(value);
    } else if (value > 0xbf && value < 0xe0) {
      str += String.fromCharCode(
        ((value & 0x1f) << 6) | (data[i + 1] & 0x3f)
      );
      i += 1;
    } else if (value > 0xdf && value < 0xf0) {
      str += String.fromCharCode(
        ((value & 0x0f) << 12) |
        ((data[i + 1] & 0x3f) << 6) |
        (data[i + 2] & 0x3f)
      );
      i += 2;
    } else {
      // surrogate pair
      var charCode =
        (((value & 0x07) << 18) |
          ((data[i + 1] & 0x3f) << 12) |
          ((data[i + 2] & 0x3f) << 6) |
          (data[i + 3] & 0x3f)) -
        0x010000;

      str += String.fromCharCode(
        (charCode >> 10) | 0xd800,
        (charCode & 0x03ff) | 0xdc00
      );
      i += 3;
    }
  }

  return str;
}

export default fromUTF8Array;