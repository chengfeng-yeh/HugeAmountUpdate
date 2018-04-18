# HugeAmountUpdate
NOTICE: Please open this project with Unity 2018.1

Test project of Unity, comparison the performance of 1. rotate 10k cubes with 10k Update(), 2. One Update() to rotate 10k cubes, and 3. Use Job System to rotate 10k cubes

1. Open the scene "RotateCubeExample"
2. Find the asset "CubeGenerator" in the scene, the default setting will create 100 X 100 cubes.
3. Find the "Rotate Method" in the Inspector, there are 3 methods: ROTATE_SELF, ROTATE_BY_SINGLE_UPDATE, and ROTATE_BY_JOB
4. You can use these 3 methods to see the difference of CPU usage by opening the profilier. On my PC, Win 10 64bit, i7-7700K, RAM 32GB, Unity 2018.1.13b, GPU instancing enabled, the result is :


| Method  | ROTATE_SELF | ROTATE_BY_SINGLE_UPDATE | ROTATE_BY_JOB |
| ------------- | ------------- | ------------- | ------------- |
| CPU time | 8.36ms | 4.61ms | 0.83ms |
