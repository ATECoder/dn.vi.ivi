#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace System;

/// <summary>   xxHash32 is used for the hash code. https://github.com/Cyan4973/xxHash. </summary>
/// <remarks>   2024-07-08. </remarks>

internal struct HashCode
{
    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1>( T1 value1 )
    {
        return value1?.GetHashCode() ?? 0;
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2>( T1 value1, T2 value2 )
    {
        return (value1, value2).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3>( T1 value1, T2 value2, T3 value3 )
    {
        return (value1, value2, value3).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <typeparam name="T4">   Generic type parameter. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <param name="value4">   The fourth value. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3, T4>( T1 value1, T2 value2, T3 value3, T4 value4 )
    {
        return (value1, value2, value3, value4).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <typeparam name="T4">   Generic type parameter. </typeparam>
    /// <typeparam name="T5">   Type of the t 5. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <param name="value4">   The fourth value. </param>
    /// <param name="value5">   The fifth value. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3, T4, T5>( T1 value1, T2 value2, T3 value3, T4 value4, T5 value5 )
    {
        return (value1, value2, value3, value4, value5).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <typeparam name="T4">   Generic type parameter. </typeparam>
    /// <typeparam name="T5">   Type of the t 5. </typeparam>
    /// <typeparam name="T6">   Type of the t 6. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <param name="value4">   The fourth value. </param>
    /// <param name="value5">   The fifth value. </param>
    /// <param name="value6">   The value 6. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3, T4, T5, T6>( T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6 )
    {
        return (value1, value2, value3, value4, value5, value6).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <typeparam name="T4">   Generic type parameter. </typeparam>
    /// <typeparam name="T5">   Type of the t 5. </typeparam>
    /// <typeparam name="T6">   Type of the t 6. </typeparam>
    /// <typeparam name="T7">   Type of the t 7. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <param name="value4">   The fourth value. </param>
    /// <param name="value5">   The fifth value. </param>
    /// <param name="value6">   The value 6. </param>
    /// <param name="value7">   The value 7. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7>( T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7 )
    {
        return (value1, value2, value3, value4, value5, value6, value7).GetHashCode();
    }

    /// <summary>   Combines the given value 1. </summary>
    /// <remarks>   2024-07-08. </remarks>
    /// <typeparam name="T1">   Generic type parameter. </typeparam>
    /// <typeparam name="T2">   Generic type parameter. </typeparam>
    /// <typeparam name="T3">   Generic type parameter. </typeparam>
    /// <typeparam name="T4">   Generic type parameter. </typeparam>
    /// <typeparam name="T5">   Type of the t 5. </typeparam>
    /// <typeparam name="T6">   Type of the t 6. </typeparam>
    /// <typeparam name="T7">   Type of the t 7. </typeparam>
    /// <typeparam name="T8">   Type of the t 8. </typeparam>
    /// <param name="value1">   The first value. </param>
    /// <param name="value2">   The second value. </param>
    /// <param name="value3">   The third value. </param>
    /// <param name="value4">   The fourth value. </param>
    /// <param name="value5">   The fifth value. </param>
    /// <param name="value6">   The value 6. </param>
    /// <param name="value7">   The value 7. </param>
    /// <param name="value8">   The value 8. </param>
    /// <returns>   An int. </returns>
    public static int Combine<T1, T2, T3, T4, T5, T6, T7, T8>( T1 value1, T2 value2, T3 value3, T4 value4, T5 value5, T6 value6, T7 value7, T8 value8 )
    {
        return (value1, value2, value3, value4, value5, value6, value7, value8).GetHashCode();
    }

}
