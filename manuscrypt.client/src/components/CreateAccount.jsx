import React, { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';

export default function CreateAccount({ onTokenReceived }) {
    const displayNameRef = useRef('');
    const emailRef = useRef('');
    const passwordRef = useRef('');
    const confirmPasswordRef = useRef('');
    const [error, setError] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsSubmitting(true);
        setError(null);

        const displayName = displayNameRef.current.value.trim();
        const email = emailRef.current.value.trim();
        const password = passwordRef.current.value;
        const confirmPassword = confirmPasswordRef.current.value;

        if (!displayName) {
            setError('Display name is required.');
            setIsSubmitting(false);
            return;
        }
        if (!email) {
            setError('Email is required.');
            setIsSubmitting(false);
            return;
        }
        if (!password) {
            setError('Password is required.');
            setIsSubmitting(false);
            return;
        }
        if (password !== confirmPassword) {
            setError('Passwords do not match.');
            setIsSubmitting(false);
            return;
        }

        try {
            const response = await fetch('http://localhost:30768/auth', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    displayName,
                    email,
                    password
                }),
            });

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to create account');
            }

            const result = await response.json();

            if (result && result.token) {
                localStorage.setItem('authToken', result.token);
                if (onTokenReceived) {
                    onTokenReceived();
                }
                console.log(result.token);
            }

            displayNameRef.current.value = '';
            emailRef.current.value = '';
            passwordRef.current.value = '';
            confirmPasswordRef.current.value = '';

            navigate("/");

        } catch (err) {
            setError(err.message);
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <form
            onSubmit={handleSubmit}
            className="bg-white rounded-2xl shadow-lg p-8 w-full max-w-md flex flex-col space-y-6"
            style={{ minWidth: 320 }}
        >
            <h2 className="text-2xl font-bold mb-2 text-center text-black">Create Account</h2>
            {error && <div className="text-red-600 text-center">{error}</div>}
            <div>
                <label htmlFor="displayName" className="block mb-1 font-medium text-black">Display Name</label>
                <input
                    id="input-displayName"
                    type="text"
                    ref={displayNameRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Enter your display name"
                    required
                />
            </div>
            <div>
                <label htmlFor="email" className="block mb-1 font-medium text-black">Email</label>
                <input
                    id="input-email"
                    type="email"
                    ref={emailRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Enter your email"
                    required
                />
            </div>
            <div>
                <label htmlFor="password" className="block mb-1 font-medium text-black">Password</label>
                <input
                    id="input-password"
                    type="password"
                    ref={passwordRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Enter your password"
                    required
                />
            </div>
            <div>
                <label htmlFor="confirmPassword" className="block mb-1 font-medium text-black">Confirm Password</label>
                <input
                    id="input-confirmPassword"
                    type="password"
                    ref={confirmPasswordRef}
                    disabled={isSubmitting}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Confirm your password"
                    required
                />
            </div>
            <button
                type="submit"
                disabled={isSubmitting}
                className="w-full bg-blue-600 text-white font-semibold py-2 rounded-lg hover:bg-blue-700 transition"
            >
                {isSubmitting ? 'Creating Account...' : 'Create Account'}
            </button>
        </form>
    );
}