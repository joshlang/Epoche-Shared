﻿using System;
using System.Text.Json;
using Xunit;

namespace Epoche.Shared.Json;

public class DecimalConverterTests
{
    static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = { new DecimalConverter() }
    };

    class TestObj
    {
        public decimal A { get; set; }
    }

    [Fact]
    public void Read_String_ReturnsValue()
    {
        var obj = JsonSerializer.Deserialize<TestObj>(@"{""A"":""1.23""}", JsonSerializerOptions);
        Assert.Equal(1.23m, obj?.A);
    }

    [Fact]
    public void Read_EmptyString_Throws() => Assert.ThrowsAny<Exception>(() => JsonSerializer.Deserialize<TestObj>(@"{""A"":""""}", JsonSerializerOptions));

    [Fact]
    public void Read_InvalidString_Throws() => Assert.ThrowsAny<Exception>(() => JsonSerializer.Deserialize<TestObj>(@"{""A"":""a""}", JsonSerializerOptions));

    [Fact]
    public void Read_NonString_Throws() => Assert.ThrowsAny<Exception>(() => JsonSerializer.Deserialize<TestObj>(@"{""A"":1}", JsonSerializerOptions));

    [Fact]
    public void Write_Roundtrips()
    {
        var test = new TestObj { A = -1.23m };
        var s = JsonSerializer.Serialize(test, JsonSerializerOptions);
        Assert.Contains(@"""-1.23""", s);
        var obj = JsonSerializer.Deserialize<TestObj>(s, JsonSerializerOptions);
        Assert.Equal(obj?.A, test.A);
    }
}
