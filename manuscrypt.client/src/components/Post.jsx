import React, { useEffect, useState } from "react";
import { GetPostedTime } from "./PostListing";
import { useParams } from "react-router-dom";

export default function Post() {
    const { postId } = useParams();
    const [post, setPost] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [comments, setComments] = useState([]);
    const [edits, setEdits] = useState([]);
    const [commentInput, setCommentInput] = useState("");
    const [editInput, setEditInput] = useState("");

    useEffect(() => {
        async function fetchPost() {
            try {
                const response = await fetch(`https://localhost:7053/post/${postId}`);
                if (!response.ok) throw new Error(await response.text());
                const data = await response.json();
                setPost(data);
                // Optionally fetch comments and edits here
            } catch (err) {
                setError(err.message);
            } finally {
                setLoading(false);
            }
        }
        fetchPost();
    }, [postId]);

    const handleSubscribe = async () => {
        alert(`Subscribed to ${post.displayName}`);
    };

    const handleAddComment = () => {
        if (commentInput.trim()) {
            setComments([...comments, { text: commentInput }]);
            setCommentInput("");
        }
    };

    const handleAddEdit = () => {
        if (editInput.trim()) {
            setEdits([...edits, { text: editInput }]);
            setEditInput("");
        }
    };

    if (loading) {
        return <div className="text-center py-8">Loading post...</div>;
    }
    if (error) {
        return <div className="text-red-600 text-center py-8">{error}</div>;
    }
    if (!post) {
        return <div className="text-gray-500 text-center py-8">Post not found.</div>;
    }

    return (
        <div className="w-full max-w-6xl mx-auto flex flex-col gap-8 py-8 px-4">
            {/* Post Info and PDF Viewer */}
            <div className="flex flex-col md:flex-row gap-8">
                {/* PDF Viewer */}
                <div className="flex-1 flex flex-col items-center">
                    <div className="w-full h-[500px] bg-gray-100 flex items-center justify-center mb-4 border rounded">
                        {post.fileUrl ? (
                            <iframe
                                src={post.fileUrl}
                                title="Manuscript PDF"
                                className="w-full h-full"
                            />
                        ) : (
                            <span className="text-gray-400">No manuscript uploaded.</span>
                        )}
                    </div>
                </div>

                {/* Post Info */}
                <div className="flex-1 flex flex-col gap-4 justify-center">
                    <div>
                        <div className="text-2xl font-bold mb-2">{post.title}</div>
                        <div className="text-gray-600 mb-1 flex items-center gap-2">
                            <span className="font-semibold">{post.displayName}</span>
                            <button
                                className="px-3 py-1 bg-blue-500 text-white rounded hover:bg-blue-600"
                                onClick={handleSubscribe}
                            >
                                Subscribe
                            </button>
                        </div>
                        <div className="text-gray-500 text-sm mb-1">
                            {GetPostedTime(post.publishedAt)}
                        </div>
                        <div className="text-gray-500 text-sm mb-1">
                            {post.views} views
                        </div>
                    </div>
                    <div className="mb-4">
                        <div className="font-semibold mb-1">Description</div>
                        <div className="text-gray-700">{post.description}</div>
                    </div>
                </div>
            </div>

            {/* Comments and Edits Section */}
            <div className="flex flex-col md:flex-row gap-8 w-full">
                {/* Comments Section */}
                <div className="flex-1 flex flex-col">
                    <div className="font-semibold mb-2">Comments</div>
                    <div className="bg-gray-50 border rounded p-2 min-h-[60px] mb-2">
                        {comments.length === 0 ? (
                            <div className="text-gray-400">No comments yet.</div>
                        ) : (
                            comments.map((comment, idx) => (
                                <div key={idx} className="mb-1">{comment.text}</div>
                            ))
                        )}
                    </div>
                    <div className="flex items-center gap-2">
                        <input
                            type="text"
                            className="flex-1 border rounded px-2 py-1"
                            placeholder="Add a comment..."
                            value={commentInput}
                            onChange={e => setCommentInput(e.target.value)}
                        />
                        <button
                            className="px-3 py-1 bg-green-500 text-white rounded hover:bg-green-600"
                            onClick={handleAddComment}
                        >
                            Add Comment
                        </button>
                    </div>
                </div>

                {/* Edits Section */}
                <div className="flex-1 flex flex-col">
                    <div className="font-semibold mb-2">Edits</div>
                    <div className="bg-gray-50 border rounded p-2 min-h-[60px] mb-2">
                        {edits.length === 0 ? (
                            <div className="text-gray-400">No edits yet.</div>
                        ) : (
                            edits.map((edit, idx) => (
                                <div key={idx} className="mb-1">{edit.text}</div>
                            ))
                        )}
                    </div>
                    <div className="flex items-center gap-2">
                        <input
                            type="text"
                            className="flex-1 border rounded px-2 py-1"
                            placeholder="Add an edit..."
                            value={editInput}
                            onChange={e => setEditInput(e.target.value)}
                        />
                        <button
                            className="px-3 py-1 bg-yellow-500 text-white rounded hover:bg-yellow-600"
                            onClick={handleAddEdit}
                        >
                            Add Edit
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}