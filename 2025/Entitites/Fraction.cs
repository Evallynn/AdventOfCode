namespace Advent;


public readonly struct Fraction : IEquatable<Fraction> {
    // Class properties
    public readonly long Numerator { get; private init; }
    public readonly long Denominator { get; private init; }
    public readonly bool IsInteger => this.Denominator == 1;


    // Class constants.
    public static readonly Fraction Zero = new(0, 1);
    public static readonly Fraction One = new(1, 1);


    // Constructors.
    public Fraction(long numerator, long denominator = 1) {
        // Prevent divide by zero.
        if (denominator == 0) throw new DivideByZeroException();

        // Invert fractions with negative denominators.
        if (denominator < 0) { numerator = -numerator; denominator = -denominator; }

        // Any fraction with a zero numerator is just zero.
        if (numerator == 0) {
            this.Numerator = 0;
            Denominator = 1;
        }
        // Otherwise, normalise the fraction and store.
        else {
            long g = GetGreatestCommonDivisor(Math.Abs(numerator), denominator);
            this.Numerator = numerator / g;
            this.Denominator = denominator / g;
        }
    }


    // Operator overrides and interface implementations.
    public static Fraction operator +(Fraction a, Fraction b)
        => new(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);

    public static Fraction operator -(Fraction a, Fraction b)
        => new(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

    public static Fraction operator *(Fraction a, Fraction b)
        => new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

    public static Fraction operator /(Fraction a, Fraction b) {
        if (b.Numerator == 0) throw new DivideByZeroException();
        return new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
    }

    public static implicit operator Fraction(int x) => new(x, 1);
    public static implicit operator Fraction(long x) => new(x, 1);

    public bool Equals(Fraction other) =>
        this.Numerator == other.Numerator && this.Denominator == other.Denominator;

    public override bool Equals(object? obj) =>
        obj is Fraction f && Equals(f);

    public override int GetHashCode() =>
        HashCode.Combine(Numerator, Denominator);

    public override string ToString() =>
        this.Denominator == 1 ? this.Numerator.ToString() : $"{this.Numerator}/{this.Denominator}";

    public long ToIntegerChecked() {
        if (!IsInteger) throw new InvalidOperationException("Fraction is not an integer");
        return Numerator;
    }


    // Class-internal utility methods.
    private static long GetGreatestCommonDivisor(long a, long b) {
        while (b != 0) {
            long t = a % b;
            a = b;
            b = t;
        }
        return a;
    }
}