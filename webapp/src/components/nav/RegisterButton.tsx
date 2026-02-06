import {Button} from "@heroui/button";
import {authConfig} from "@/lib/config";

export default function RegisterButton() {
    const clientId = authConfig.kcClientId;
    const issuer = authConfig.kcIssuer;
    const redirectUrl = authConfig.authUrl;
    
    const registerUrl = `${issuer}/protocol/openid-connect/registrations` +
        `?client_id=${clientId}&redirect_uri=${encodeURIComponent(redirectUrl!)}` +
        `&response_type=code&scope=openid`;
    return (
        <Button as='a' href={registerUrl} color="secondary">Register</Button>
    );
}