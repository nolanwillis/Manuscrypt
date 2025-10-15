import React, { useState, useRef } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Login({ onUserIdReceived }) {
    const emailRef = useRef('');
    const passwordRef = useRef('');
    const [error, setError] = useState(null);
    const [isLoginSubmitted, setIsLoginSubmitted] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoginSubmitted(true);
        setError(null);
        try {
            const email = emailRef.current.value;
            if (!email) {
                throw new Error('Email is required.');
            }
            const password = passwordRef.current.value;
            if (!password) {
                throw new Error('Password is required.');
            }
            const response = await fetch('http://localhost:5125/user/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    email,
                    password
                }),
            });

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to login');
            }

            const user = await response.json();

            if (user && user.id && onUserIdReceived) {
                onUserIdReceived(user.id);
            }

            emailRef.current.value = '';
            passwordRef.current.value = '';

            navigate("/");

        } catch (err) {
            setError(err.message);
        } finally {
            setIsLoginSubmitted(false);
        }
    }

    return (
        <form
            onSubmit={handleSubmit}
            className="bg-white rounded-2xl shadow-lg p-8 w-full max-w-md flex flex-col space-y-6"
            style={{ minWidth: 320 }}
        >
            <h2 className="text-2xl font-bold mb-2 text-center text-black">Login</h2>
            {error && <div className="text-red-600 text-center">{error}</div>}
            <div>
                <label htmlFor="email" className="block mb-1 font-medium text-black">Email</label>
                <input
                    id="input-email"
                    type="email"
                    ref={emailRef}
                    disabled={isLoginSubmitted}
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
                    disabled={isLoginSubmitted}
                    className="w-full px-4 py-2 rounded-lg border border-gray-300 focus:outline-none focus:ring-2 focus:ring-blue-400 text-black"
                    placeholder="Enter your password"
                    required
                />
            </div>
            <button
                type="submit"
                disabled={isLoginSubmitted}
                className="w-full bg-blue-600 text-white font-semibold py-2 rounded-lg hover:bg-blue-700 transition"
            >
                {isLoginSubmitted ? 'Logging In...' : 'Login'}
            </button>
        </form>
    );
}
