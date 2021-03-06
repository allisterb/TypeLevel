﻿namespace TypeLevel

[<AutoOpen>]
module Nat =
  type Nat = interface end
  type Z = Z with interface Nat
  type S<'a> = S of 'a with interface Nat

  type NatPair<'a, 'b> = NatPair of 'a * 'b

  type Z with
    static member Zero = Z
    static member One = S Z
    static member inline (+) (Z, x) = x
    static member inline (-) (x, Z) = x
    static member inline (-) (Z, x) = Z
    static member inline ( * ) (Z, x) = Z
    static member inline (<<=) (Z, x) = True
    static member inline (===) (Z, Z) = True
    static member inline ( <|*- ) (Z, (f, x)) = x
    static member inline (!++) Z = (S Z)
    static member inline (!--) Z = Z

    static member inline (!?) Z = False

  type S<'a> with
    static member Zero = Z
    static member One = S Z
    static member inline (+) (S a, x) = S(a + x)
    static member inline (-) (S x, S y) = x - y
    static member inline ( * ) (S a, b) = a + a * b
    static member inline (<<=) (S b, Z) = False
    static member inline (<<=) (S b, S c) = b <<= c

    //static member inline (===) (Z, S _) = False
    //static member inline (===) (S _, Z) = False
    //static member inline (===) (S a, S b) = a === b

    // Unrolled rules to improve performance
    static member inline (===) (S (S a), S (S b)) = a === b
    static member inline (===) (S (S a), S Z) = False
    static member inline (===) (S Z, S (S a)) = False
    static member inline (===) (S Z, S Z) = True

    static member inline (<|*-) (S n, (f, x)) = n <|*- (f, f <|- x)

    static member inline (!++) x = S x
    static member inline (!--) (S x) = x

    static member inline (!?) (S _) = True

  type N0 = Z
  type N1 = S<N0>
  type N2 = S<N1>
  type N3 = S<N2>
  type N4 = S<N3>
  type N5 = S<N4>
  type N6 = S<N5>
  type N7 = S<N6>
  type N8 = S<N7>
  type N9 = S<N8>
  type N10 = S<N9>
  type N11 = S<N10>
  type N12 = S<N11>
  type N13 = S<N12>
  type N14 = S<N13>
  type N15 = S<N14>
  type N16 = S<N15>
  type N17 = S<N16>
  type N18 = S<N17>
  type N19 = S<N18>
  type N20 = S<N19>

  let N0 : N0 = Z
  let N1 : N1 = S N0
  let N2 : N2 = S N1
  let N3 : N3 = S N2
  let N4 : N4 = S N3
  let N5 : N5 = S N4
  let N6 : N6 = S N5
  let N7 : N7 = S N6
  let N8 : N8 = S N7
  let N9 : N9 = S N8
  let N10 : N10 = S N9
  let N11 : N11 = S N10
  let N12 : N12 = S N11
  let N13 : N13 = S N12
  let N14 : N14 = S N13
  let N15 : N15 = S N14
  let N16 : N16 = S N15
  let N17 : N17 = S N16
  let N18 : N18 = S N17
  let N19 : N19 = S N18
  let N20 : N20 = S N19

  type Pred = Pred with
    interface Func
    static member inline (<|-) (Pred, Z) = Z
    static member inline (<|-) (Pred, S x) = x

  type Succ = Succ with
    interface Func
    static member inline (<|-) (Succ, x) = S x

  type IsZero = IsZero with
    interface Func
    static member inline (<|-) (IsZero, Z) = True
    static member inline (<|-) (IsZero, S (_ : ^a)) = False

  type RepHelper<'f, 'x> = RepHelper of 'f * 'x

  type Z with
    static member inline (<***>) (Z, RepHelper(f, x)) = x

  type S<'a> with
    static member inline (<***>) (S n, RepHelper(f, x)) =
      n <***> RepHelper(f, f <|- x)

  type Rep<'n, 'f> = Rep of 'n * 'f with
    interface Func
    static member inline (<|-) (Rep(n, f), x) =
      n <***> RepHelper(f, x)

  type Repeat<'f> = Repeat of 'f with
    interface Func
    static member inline (<|-) (Repeat f, (n, x)) =
      n <|*- (f, x)

  type Add = Add with
    interface Func
    static member inline (<|-) (Add, (x, y)) = x + y

  type Sub = Sub with
    interface Func
    static member inline (<|-)(Sub, (x, y)) =
      x - y

  let inline Plus(x) = Partial(Add, x)

  type Mult = Mult with
    interface Func
    static member inline (<|-) (Mult, (x, y)) =
      x <***> RepHelper(Plus(y), Z)

  let inline Times(x) = Partial(Mult, x)

  type Pow = Pow with
    interface Func
    static member inline (<|-) (Pow, (x, y)) =
      y <***> RepHelper(Times(x), N1)

  type LessEq = LessEq with
    interface Func
    static member inline (<|-) (LessEq, (x, y)) =
      x <<= y

  type GreaterEq = GreaterEq with
    interface Func
    static member inline (<|-) (GreaterEq, (x, y)) =
      y <<= x

  type Eq = Eq with
    interface Func
    static member inline (<|-) (Eq, (x, y)) =
      x === y
