﻿using System.Security.Cryptography;

namespace SampleDocker;

public static class SecureData
{
	private static RSA rsa;

	public static RSA Rsa => rsa ??= RSA.Create(2048);
}
