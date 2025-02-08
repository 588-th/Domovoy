shader_type spatial;

uniform sampler2D texture_albedo : source_color, filter_nearest;
uniform sampler2D texture_noise : filter_nearest;
uniform float intensity : hint_range(0.0, 1.0) = 0.5;
uniform float resolution_x : hint_range(1.0, 1024.0) = 256.0;
uniform float resolution_y : hint_range(1.0, 1024.0) = 256.0;
uniform float speed : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    vec2 uv = UV;
    uv.x = floor(uv.x * resolution_x) / resolution_x;
    uv.y = floor(uv.y * resolution_y) / resolution_y;

    vec4 albedo_color = texture(texture_albedo, uv);

    float time = TIME * speed;
    vec2 noise_uv = vec2(uv.x + time, uv.y + time);

    float noise_value = texture(texture_noise, noise_uv).r;

    vec4 final_color = mix(albedo_color, vec4(1.0), noise_value * intensity);

    ALBEDO = final_color.rgb;
}
