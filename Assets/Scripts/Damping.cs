using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct OmegaAndB
{
    public float Omega;
    public float B;
    public OmegaAndB(float omega, float b)
    {
        Omega = omega;
        B = b;
    }
}

public class Damping
{
    private float _A, _B, _omega, _omega_d, _zeta;

    public Damping(float damping, int halfcycles, float initialValue = 1, float inititalVelocity = 0)
    {
        _zeta = damping;
        float k = halfcycles;
        float y0 = initialValue;
        float v0 = inititalVelocity;
        _A = initialValue;

        if (Mathf.Abs(v0) < 0.0000001f)
        {
            _B = _zeta * y0 / Mathf.Sqrt(1 - _zeta * _zeta);
            _omega = ComputeOmega(_A, _B, k, _zeta);
        }
        else
        {
            var result = NumericallySolveOmegaAndB(_zeta, k, y0, v0);
            _B = result.B;
            _omega = result.Omega;
        }

        _omega *= 2 * Mathf.PI;
        _omega_d = _omega * Mathf.Sqrt(1 - _zeta * _zeta);
    }

    public float GetValue(float t)
    {
        float sinusoid = _A * Mathf.Cos(_omega_d * t) + _B * Mathf.Sin(_omega_d * t);
        return Mathf.Exp(-t * _zeta * _omega) * sinusoid;
    }


    private OmegaAndB NumericallySolveOmegaAndB(float zeta, float k, float y0, float v0)
    {
        //float errorfn(float B, float omega)
        //{
        //    var omega_d = omega * Mathf.Sqrt(1 - zeta * zeta);
        //    return B - ((zeta * omega * y0) + v0) / omega_d;
        //}

        float A = y0;
        float B = zeta;

        float omega, error, direction;

        void step()
        {
            omega = ComputeOmega(A, B, k, zeta);
            //error = errorfn(B, omega);
            var omega_d = omega * Mathf.Sqrt(1 - zeta * zeta);
            error = B - ((zeta * omega * y0) + v0) / omega_d;
            direction = -Mathf.Sign(error);
        }

        step();

        float tolerence = 0.0000001f;
        float lower = 0, upper;
        float ct = 0;
        float maxct = 1000;

        if (direction > 0)
        {
            while (direction > 0)
            {
                ct++;

                if (ct > maxct)
                {
                    break;
                }

                lower = B;

                B *= 2;
                step();
            }

            upper = B;
        }
        else
        {
            upper = B;

            B *= -1;

            while (direction < 0)
            {
                ct++;

                if (ct > maxct)
                {
                    break;
                }

                lower = B;

                B *= 2;
                step();
            }

            lower = B;
        }

        while (Mathf.Abs(error) > tolerence)
        {
            ct++;

            if (ct > maxct)
            {
                break;
            }

            B = (upper + lower) / 2;
            step();

            if (direction > 0)
            {
                lower = B;
            }
            else
            {
                upper = B;
            }
        }

        return new OmegaAndB(omega, B);
    }

    private float ComputeOmega(float A, float B, float k, float zeta)
    {
        if (A * B < 0 && k >= 1)
            k--;

        return (-Mathf.Atan(A / B) + Mathf.PI * k) / (2 * Mathf.PI * Mathf.Sqrt(1 - zeta * zeta));
    }
}
