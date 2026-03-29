type ErrorMessageProps = {
  error?: string;
};

export default function ErrorMessage({ error }: ErrorMessageProps) {
  if (!error) {
    return null;
  }

  return <p className="text-sm text-rose-600">{error}</p>;
}
