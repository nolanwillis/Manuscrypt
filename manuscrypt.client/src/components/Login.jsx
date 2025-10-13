import React, { useState, useRef } from 'react';

export default function Login({ onUserFound }) {
    const emailRef = useRef('');
    const [error, setError] = useState(null);
    const [isLoginSubmitted, setIsLoginSubmitted] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setIsLoginSubmitted(true);
        setError(null);
        try {
            const email = emailRef.current.value;
            if (!email) {
                throw new Error('Email is required.');
            }

            const response = await fetch(`http://localhost:5125/User?email=${encodeURIComponent(email)}`);

            if (!response.ok) {
                const errorMsg = await response.text();
                throw new Error(errorMsg || 'Failed to login');
            }

            const user = await response.json();

            // Store the user ID in session storage
            if (user && user.id) {
                sessionStorage.setItem('userId', user.id);
            }

            emailRef.current.value = '';
            if (onUserFound) {
                onUserFound(user.id);
            }
        } catch (err) {
            setError(err.message);
        } finally {
            setIsLoginSubmitted(false);
        }
    }

    return ( 
        <div className="h-auto w-auto flex items-center justify-center bg-white">
            <div className="w-full max-w-md p-8 border-2 border-black">
                <h1 className="text-xl font-bold mb-6 text-black">Login</h1>

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label htmlFor="email" className="text-black">Email</label>
                        <input
                          id="input-email"
                          type="email"
                          ref={emailRef}
                          disabled={isLoginSubmitted}
                          className="border-2 border-black bg-white text-black"
                          required
                        />
                    </div>
                        
                    <button 
                    type="submit" 
                    disabled={isLoginSubmitted}
                    className="w-full bg-black text-white hover:bg-gray-800"
                    >
                        {isLoginSubmitted ? 'Logging In...' : 'Login'}
                    </button>
                </form>
            </div>
        </div>
    );
};
