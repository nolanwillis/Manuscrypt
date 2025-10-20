import React, { useState, useEffect } from 'react';
import { useNavigate, Link } from "react-router-dom";

export default function NavBar({ isLoggedIn, onLogoutClicked }) {
    const navigate = useNavigate();

    const handleLogin = () => {
        navigate("/login");
    }
     const handleCreateAccount = () => {
        navigate("/create-account");
    }
    
    return (
        <nav className="fixed top-0 left-0 bottom-0 w-64 z-50 bg-background/80 border-r flex flex-col">
            <div className="p-6 border-b">
                <span className="font-manuscript text-2xl font-bold text-foreground">
                    Manuscrypt
                </span>
            </div>

            <div className="flex-1 flex flex-col justify-between py-6">
                {isLoggedIn && (
                    <div className="space-y-2 px-4">
                        <Link to="/" className="block px-4 py-3 rounded-lg">
                            Home
                        </Link>
                        <Link to="/subscriptions" className="block px-4 py-3 rounded-lg">
                            Subscriptions
                        </Link>
                        <Link to="/my-account" className="block px-4 py-3 rounded-lg">
                            My Account
                        </Link>
                        <Link to="/create-post" className="block px-4 py-3 rounded-lg">
                            Create Post
                        </Link>
                    </div>
                )}
                <div className="space-y-2 px-4">
                    <button className="w-full px-4 py-2 rounded-lg">
                        Settings
                    </button>
                    {isLoggedIn ? (
                        <button
                            className="w-full px-4 py-2 rounded-lg"
                            onClick={() => {
                                onLogoutClicked();
                                navigate("/");
                            }}
                        >
                            Logout
                        </button>
                    ) : (
                        <div> 
                            <button
                                    className="w-full px-4 py-2 rounded-lg"
                                    onClick={handleLogin}
                            >
                                Login
                            </button>
                            <button
                                    className="w-full px-4 py-2 rounded-lg"
                                    onClick={handleCreateAccount}
                            >
                                Create Account
                            </button>
                        </div>

                    )}
                </div>
            </div>
        </nav>
    );
}