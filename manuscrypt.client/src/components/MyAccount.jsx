import React, { useEffect, useState } from "react";
import PostListing from "./PostListing";

export default function MyAccount() {
    const [user, setUser] = useState(null);
    const [posts, setPosts] = useState([]);
    const [error, setError] = useState(null);
    const [loading, setLoading] = useState(true);

    async function FetchUserAndPosts() {
        try {
            const userId = sessionStorage.getItem("userId");
            if (!userId) throw new Error("User ID not found in session.");

            const userResponse = await fetch(`https://localhost:7053/user/${userId}`);
            if (!userResponse.ok) throw new Error(await userResponse.text());
            const userData = await userResponse.json();
            setUser(userData);

            const postsResponse = await fetch(`https://localhost:7053/user/${userId}/posts`);
            if (!postsResponse.ok) throw new Error(await postsResponse.text());
            const postsData = await postsResponse.json();
            setPosts(postsData);

        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        FetchUserAndPosts();
    }, []);

    if (loading) {
        return <div className="text-center py-8">Loading account...</div>;
    }
    if (error) {
        return <div className="text-red-600 text-center py-8">{error}</div>;
    }
    if (!user) {
        return <div className="text-gray-500 text-center py-8">No user data found.</div>;
    }

    return (
        <div className="w-full h-[calc(100vh-100px)] overflow-y-auto px-4 flex flex-col items-center">
            {/* User Info Section */}
            <div className="w-full max-w-xl flex flex-col items-center py-8">
                <div className="w-24 h-24 rounded-full bg-gray-200 flex items-center justify-center overflow-hidden mb-4">
                    {user.photoUrl ? (
                        <img
                            src={user.photoUrl}
                            alt="User"
                            className="w-full h-full object-cover"
                        />
                    ) : (
                        <span className="text-4xl text-gray-400">
                            {user.displayName ? user.displayName[0] : "?"}
                        </span>
                    )}
                </div>
                <div className="text-2xl font-semibold mb-2">{user.displayName}</div>
                <div className="text-gray-600 text-center mb-4">{user.description}</div>
            </div>

            {/* Posts Section */}
            <div className="w-full max-w-4xl flex flex-wrap gap-y-2 gap-x-4 justify-center">
                {(!posts || posts.length === 0) ? (
                    <div className="text-gray-500 text-center w-full">No posts found.</div>
                ) : (
                    posts.map((post) => (
                        <PostListing
                            post={{
                                id: post.id,
                                displayName: post.displayName,
                                title: post.title,
                                description: post.description,
                                publishedAt: post.publishedAt,
                                views: post.views,
                                fileUrl: post.fileUrl
                            }}
                        />
                    ))
                )}
            </div>
        </div>
    );
}