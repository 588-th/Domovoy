shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture;

uniform bool enable_dithering = true;
uniform float dithering_intensity : hint_range(0.0, 1.0) = 0.5;
uniform float dither_scale : hint_range(0.5, 10.0) = 1.0;

const float dither8x8[64] = float[](
    0.0 / 64.0, 32.0 / 64.0, 8.0 / 64.0, 40.0 / 64.0, 2.0 / 64.0, 34.0 / 64.0, 10.0 / 64.0, 42.0 / 64.0,
    48.0 / 64.0, 16.0 / 64.0, 56.0 / 64.0, 24.0 / 64.0, 50.0 / 64.0, 18.0 / 64.0, 58.0 / 64.0, 26.0 / 64.0,
    12.0 / 64.0, 44.0 / 64.0, 4.0 / 64.0, 36.0 / 64.0, 14.0 / 64.0, 46.0 / 64.0, 6.0 / 64.0, 38.0 / 64.0,
    60.0 / 64.0, 28.0 / 64.0, 52.0 / 64.0, 20.0 / 64.0, 62.0 / 64.0, 30.0 / 64.0, 54.0 / 64.0, 22.0 / 64.0,
    3.0 / 64.0, 35.0 / 64.0, 11.0 / 64.0, 43.0 / 64.0, 1.0 / 64.0, 33.0 / 64.0, 9.0 / 64.0, 41.0 / 64.0,
    51.0 / 64.0, 19.0 / 64.0, 59.0 / 64.0, 27.0 / 64.0, 49.0 / 64.0, 17.0 / 64.0, 57.0 / 64.0, 25.0 / 64.0,
    15.0 / 64.0, 47.0 / 64.0, 7.0 / 64.0, 39.0 / 64.0, 13.0 / 64.0, 45.0 / 64.0, 5.0 / 64.0, 37.0 / 64.0,
    63.0 / 64.0, 31.0 / 64.0, 55.0 / 64.0, 23.0 / 64.0, 61.0 / 64.0, 29.0 / 64.0, 53.0 / 64.0, 21.0 / 64.0
);

void fragment() {
    vec4 screen_color = texture(screen_texture, SCREEN_UV);

    float brightness = dot(screen_color.rgb, vec3(0.299, 0.587, 0.114));
    float dithered_brightness = brightness;

    if (enable_dithering) {
        ivec2 coords = ivec2(FRAGCOORD.xy / dither_scale) % 8;
        int index = coords.y * 8 + coords.x;
        float threshold = dither8x8[index];
        dithered_brightness = smoothstep(threshold - 0.1, threshold + 0.1, brightness);
    }

    vec3 dither_pattern = vec3(dithered_brightness);
    vec3 final_color = mix(screen_color.rgb, dither_pattern, dithering_intensity);

    COLOR = vec4(final_color, screen_color.a);
}
